using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;
using PrankChat.Mobile.Core.BusinessServices.Sentry;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling
{
    public class ErrorHandleService : IErrorHandleService
    {
        private const int ZeroSkipDelay = 0;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IDialogService _dialogService;
        private readonly IMvxLogProvider _logProvider;

        private readonly Lazy<ICrashlyticsService> _lazyCrashlyticsService = new Lazy<ICrashlyticsService>(() => Mvx.IoCProvider.Resolve<ICrashlyticsService>());
        private readonly Lazy<ISentryService> _lazySentryService = new Lazy<ISentryService>(() => Mvx.IoCProvider.Resolve<ISentryService>());

        private bool _isSuspended;

        public ErrorHandleService(IMvxMessenger messenger, IDialogService dialogService, IMvxLogProvider logProvider)
        {
            _dialogService = dialogService;
            _logProvider = logProvider;

            messenger.Subscribe<ServerErrorMessage>(OnServerErrorEvent, MvxReference.Strong);
        }

        public void HandleException(Exception exception)
        {
            switch (exception)
            {
                case NetworkException networkException when !string.IsNullOrWhiteSpace(networkException.Message):
                    DisplayMessage(() => _dialogService.ShowToast(networkException.Message, ToastType.Negative));
                    break;

                case ValidationException validationException:
                    var loacalizedMessage = GetValidationErrorLocalizedMessage(validationException);
                    DisplayMessage(() => _dialogService.ShowToast(loacalizedMessage, ToastType.Negative));
                    break;

                case BaseUserVisibleException userException when !string.IsNullOrWhiteSpace(userException.Message):
                    DisplayMessage(() => _dialogService.ShowToast(userException.Message, ToastType.Negative));
                    break;

                case Exception ex:
                    var message = string.IsNullOrWhiteSpace(ex.Message) ? Resources.Error_Unexpected_Network : ex.Message;
                    DisplayMessage(() => _dialogService.ShowToast(message, ToastType.Negative));
                    _lazySentryService.Value.TrackError(exception);
                    break;
            }
        }

        public void LogError(object sender, string message, Exception exception = null)
        {
            var senderType = sender.GetType();
            var logger = _logProvider.GetLogFor(senderType);
            logger.Log(MvxLogLevel.Error, () => message, exception);
            _lazySentryService.Value.TrackEvent(message);

            if (exception != null)
            {
                _lazySentryService.Value.TrackError(exception);
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
                    DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Server));
                    return;

                case ProblemDetailsException problemDetails:
                    if (string.IsNullOrWhiteSpace(problemDetails.Message) &&
                        string.IsNullOrWhiteSpace(problemDetails.Title))
                    {
                        DisplayMessage(() => _dialogService.ShowToast(Resources.Error_Unexpected_Network, ToastType.Negative));
                        return;
                    }

                    DisplayMessage(() => _dialogService.ShowToast(problemDetails.Message ?? problemDetails.Title, ToastType.Negative));
                    break;

                case NullReferenceException nullReference:
                    _lazyCrashlyticsService.Value.TrackError(nullReference);
                    _lazySentryService.Value.TrackError(nullReference);
                    break;

                case Exception ex when ex.InnerException is NullReferenceException:
                    _lazyCrashlyticsService.Value.TrackError(ex);
                    _lazySentryService.Value.TrackError(ex);
                    break;

                case Exception ex when ex.InnerException != null:
                    var message = ex.InnerException.Message ?? Resources.Error_Unexpected_Server;
                    DisplayMessage(() => _dialogService.ShowToast(message, ToastType.Negative));
                    break;

                case Exception ex when ex.GetType() != typeof(NullReferenceException):
                    var errorMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : Resources.Error_Unexpected_Server;
                    DisplayMessage(async () => await _dialogService.ShowAlertAsync(errorMessage));
                    break;
            }
        }

        private string GetValidationErrorLocalizedMessage(ValidationException exception)
        {
            switch (exception.ErrorType)
            {
                case ValidationErrorType.Empty:
                    return string.Format(Resources.Validation_Error_Empty, exception.LocalizedFieldName);

                case ValidationErrorType.CanNotMatch:
                    return string.Format(Resources.Validation_Error_CanNotMatch, exception.LocalizedFieldName, exception.RelativeValue);

                case ValidationErrorType.GreaterThanRequired:
                    return string.Format(Resources.Validation_Error_GreaterThanRequired, exception.LocalizedFieldName, exception.RelativeValue);

                case ValidationErrorType.LowerThanRequired:
                    return string.Format(Resources.Validation_Error_LowerThanRequired, exception.LocalizedFieldName, exception.RelativeValue);

                case ValidationErrorType.NotMatch:
                    return string.Format(Resources.Validation_Error_NotMatch, exception.LocalizedFieldName, exception.RelativeValue);

                case ValidationErrorType.Invalid:
                    return string.Format(Resources.Validation_Error_Invalid, exception.LocalizedFieldName);

                case ValidationErrorType.NotConfirmed:
                    return string.Format(Resources.Not_Confirmed, exception.LocalizedFieldName);

                case ValidationErrorType.Undefined:
                    return exception.Message;

                default:
                    return string.Empty;
            }
        }

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
