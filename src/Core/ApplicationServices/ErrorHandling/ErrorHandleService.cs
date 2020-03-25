using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling
{
    public class ErrorHandleService : IErrorHandleService
    {
        private const int ZeroSkipDelay = 0;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IDialogService _dialogService;
        private readonly IMvxLogProvider _logProvider;

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
                case ValidationException validationException:
                    var message = GetValidationErrorLocalizedMessage(validationException);
                    _dialogService.ShowToast(message, ToastType.Negative);
                    break;

                case BaseUserVisibleException ex when !string.IsNullOrWhiteSpace(ex.Message):
                    _dialogService.ShowToast(ex.Message, ToastType.Negative);
                    break;
            }
        }

        public void LogError(object sender, string message, Exception exception = null)
        {
            var senderType = sender.GetType();
            var logger = _logProvider.GetLogFor(senderType);
            logger.Log(MvxLogLevel.Error, () => message, exception);
        }

        private void OnServerErrorEvent(ServerErrorMessage e)
        {
            var exception = e.Error;

            switch (exception)
            {
                case NetworkException networkException:
                    DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Server));
                    return;

                case ProblemDetailsDataModel problemDetails:
                    if (string.IsNullOrWhiteSpace(problemDetails.Message))
                    {
                        _dialogService.ShowToast(Resources.Error_Unexpected_Network, ToastType.Negative);
                        return;
                    }

                    _dialogService.ShowToast(problemDetails.Message, ToastType.Negative);
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
    }
}
