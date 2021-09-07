using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using PrankChat.Mobile.Core.Plugins.UserInteraction;

namespace PrankChat.Mobile.Core.Services.ErrorHandling
{
    public class ErrorHandleService : IErrorHandleService
    {
        private const int ZeroSkipDelay = 0;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IUserInteraction _userInteraction;
        private readonly IMvxLogProvider _logProvider;
        private bool _isSuspended;

        public ErrorHandleService(
            IMvxMessenger messenger,
            IUserInteraction userInteraction,
            IMvxLogProvider logProvider)
        {
            _userInteraction = userInteraction;
            _logProvider = logProvider;

            messenger.Subscribe<ServerErrorMessage>(OnServerErrorEvent, MvxReference.Strong);
        }

        public void HandleException(Exception exception)
        {
            switch (exception)
            {
                case NetworkException networkException when !string.IsNullOrWhiteSpace(networkException.Message):
                    DisplayMessage(() => _userInteraction.ShowToast(networkException.Message, ToastType.Negative));
                    break;

                case ValidationException validationException:
                    var loacalizedMessage = GetValidationErrorLocalizedMessage(validationException);
                    DisplayMessage(() => _userInteraction.ShowToast(loacalizedMessage, ToastType.Negative));
                    break;

                case BaseUserVisibleException userException when !string.IsNullOrWhiteSpace(userException.Message):
                    DisplayMessage(() => _userInteraction.ShowToast(userException.Message, ToastType.Negative));
                    break;

                case Exception ex:
                    var message = string.IsNullOrWhiteSpace(ex.Message) ? Resources.Error_Unexpected_Network : ex.Message;
                    DisplayMessage(() => _userInteraction.ShowToast(message, ToastType.Negative));
                    Crashes.TrackError(exception);
                    break;
            }
        }

        public void LogError(object sender, string message, Exception exception = null)
        {
            var senderType = sender.GetType();
            var logger = _logProvider.GetLogFor(senderType);
            logger.Log(MvxLogLevel.Error, () => message, exception);
            Analytics.TrackEvent(message);

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
                    DisplayMessage(async () => await _userInteraction.ShowAlertAsync(Resources.Error_Unexpected_Server));
                    return;

                case ProblemDetailsException problemDetails:
                    if (string.IsNullOrWhiteSpace(problemDetails.Message) &&
                        string.IsNullOrWhiteSpace(problemDetails.Title))
                    {
                        DisplayMessage(() => _userInteraction.ShowToast(Resources.Error_Unexpected_Network, ToastType.Negative));
                        return;
                    }

                    DisplayMessage(() => _userInteraction.ShowToast(problemDetails.Message ?? problemDetails.Title, ToastType.Negative));
                    break;

                case NullReferenceException nullReference:
                    Crashes.TrackError(nullReference);
                    break;

                case Exception ex when ex.InnerException is NullReferenceException:
                    Crashes.TrackError(ex);
                    break;

                case Exception ex when ex.InnerException != null:
                    var message = ex.InnerException.Message ?? Resources.Error_Unexpected_Server;
                    DisplayMessage(() => _userInteraction.ShowToast(message, ToastType.Negative));
                    break;

                case Exception ex when ex.GetType() != typeof(NullReferenceException):
                    var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : Resources.Error_Unexpected_Server;
                    DisplayMessage(async () => await _userInteraction.ShowAlertAsync(errorMessage));
                    break;
            }
        }

        private string GetValidationErrorLocalizedMessage(ValidationException exception) => exception.ErrorType switch
        {
            ValidationErrorType.Empty => string.Format(Resources.Validation_Error_Empty, exception.LocalizedFieldName),
            ValidationErrorType.CanNotMatch => string.Format(Resources.Validation_Error_CanNotMatch, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.GreaterThanRequired => string.Format(Resources.Validation_Error_GreaterThanRequired, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.LowerThanRequired => string.Format(Resources.Validation_Error_LowerThanRequired, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.NotMatch => string.Format(Resources.Validation_Error_NotMatch, exception.LocalizedFieldName, exception.RelativeValue),
            ValidationErrorType.Invalid => string.Format(Resources.Validation_Error_Invalid, exception.LocalizedFieldName),
            ValidationErrorType.NotConfirmed => string.Format(Resources.Not_Confirmed, exception.LocalizedFieldName),
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
