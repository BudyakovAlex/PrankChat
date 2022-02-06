using Microsoft.AppCenter.Crashes;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Services.ErrorHandling
{
    public class ErrorHandleService : IErrorHandleService
    {
        private const int ZeroSkipDelay = 0;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private bool _isSuspended;

        // This need for normal initialize ErrorHandleService on start application, because IUserInteraction on this time not register.
        private IUserInteraction UserInteraction => CompositionRoot.Container.Resolve<IUserInteraction>();

        public ErrorHandleService(IMvxMessenger messenger)
        {
            messenger.Subscribe<ServerErrorMessage>(OnServerErrorEvent, MvxReference.Strong);
        }

        public void HandleException(Exception exception)
        {
            switch (exception)
            {
                case NetworkException networkException when !string.IsNullOrWhiteSpace(networkException.Message):
                    DisplayMessage(() => UserInteraction.ShowToast(networkException.Message, ToastType.Negative));
                    break;

                case ValidationException validationException:
                    var loacalizedMessage = GetValidationErrorLocalizedMessage(validationException);
                    DisplayMessage(() => UserInteraction.ShowToast(loacalizedMessage, ToastType.Negative));
                    break;

                case BaseUserVisibleException userException when !string.IsNullOrWhiteSpace(userException.Message):
                    DisplayMessage(() => UserInteraction.ShowToast(userException.Message, ToastType.Negative));
                    break;

                case Exception ex:
                    var message = string.IsNullOrWhiteSpace(ex.Message) ? Resources.ErrorUnexpectedNetwork : ex.Message;
                    DisplayMessage(() => UserInteraction.ShowToast(message, ToastType.Negative));
                    Crashes.TrackError(exception);
                    break;
            }
        }

        public void LogError(object sender, string message, Exception exception = null)
        {
            var logger = sender.Logger();
            logger.Write(Serilog.Events.LogEventLevel.Error, exception, message);
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(message);

            if (exception != null)
            {
                Crashes.TrackError(exception);
            }
        }

        private void OnServerErrorEvent(ServerErrorMessage e)
        {
            if (_isSuspended)
            {
                return;
            }

            var exception = e.Error;

            switch (exception)
            {
                case NetworkException networkException:
                    DisplayMessage(async () => await UserInteraction.ShowAlertAsync(Resources.ErrorUnexpectedServer));
                    return;

                case ProblemDetailsException problemDetails:
                    if (string.IsNullOrWhiteSpace(problemDetails.Message) &&
                        string.IsNullOrWhiteSpace(problemDetails.Title))
                    {
                        DisplayMessage(() => UserInteraction.ShowToast(Resources.ErrorUnexpectedNetwork, ToastType.Negative));
                        return;
                    }

                    DisplayMessage(() => UserInteraction.ShowToast(problemDetails.Message ?? problemDetails.Title, ToastType.Negative));
                    break;

                case NullReferenceException nullReference:
                    Crashes.TrackError(nullReference);
                    break;

                case Exception ex when ex.InnerException is NullReferenceException:
                    Crashes.TrackError(ex);
                    break;

                case Exception ex when ex.InnerException != null:
                    var message = ex.InnerException.Message ?? Resources.ErrorUnexpectedServer;
                    DisplayMessage(() => UserInteraction.ShowToast(message, ToastType.Negative));
                    break;

                case Exception ex when ex.GetType() != typeof(NullReferenceException):
                    var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : Resources.ErrorUnexpectedServer;
                    DisplayMessage(async () => await UserInteraction.ShowAlertAsync(errorMessage));
                    break;
            }
        }

        private string GetValidationErrorLocalizedMessage(ValidationException exception) => exception.ErrorType switch
        {
            ValidationErrorType.Empty => string.Format(Resources.CannotBeEmpty, exception.LocalizedFieldName),
            ValidationErrorType.CanNotMatch => string.Format(Resources.ValidationErrorCanNotMatch, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.GreaterThanRequired => string.Format(Resources.ValidationErrorGreaterThanRequired, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.LowerThanRequired => string.Format(Resources.ValidationErrorLowerThanRequired, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.NotMatch => string.Format(Resources.ValidationErrorNotMatch, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.Invalid => string.Format(Resources.ValidationErrorInvalid, exception.LocalizedFieldName),
            ValidationErrorType.NotConfirmed => string.Format(Resources.NotVerified, exception.LocalizedFieldName),
            ValidationErrorType.Undefined => exception.Message,
            _ => string.Empty,
        };


        private void DisplayMessage(Func<Task> messageAction)
        {
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await _semaphore.WaitAsync(ZeroSkipDelay))
                {
                    return;
                }

                try
                {
                    await messageAction.Invoke();
                }
                finally
                {
                    _semaphore.Release();
                }
            });
        }

        private void DisplayMessage(Action messageAction)
        {
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await _semaphore.WaitAsync(ZeroSkipDelay))
                {
                    return;
                }
                try
                {
                    messageAction.Invoke();
                }
                finally
                {
                    _semaphore.Release();
                }
            });
        }

        public void SuspendServerErrorsHandling()
        {
            _isSuspended = true;
        }

        public void ResumeServerErrorsHandling()
        {
            _isSuspended = false;
        }
    }
}
