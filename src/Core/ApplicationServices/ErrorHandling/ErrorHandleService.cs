using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
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
        private const string ListMark = "-";
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
                    DisplayMessage(async () => _dialogService.ShowToast(message, ToastType.Negative));
                    break;

                case BaseUserVisibleException _:
                    DisplayMessage(async () => _dialogService.ShowToast(exception.Message, ToastType.Negative));
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
                case NetworkException _:
                    DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Server));
                    return;

                case ProblemDetails problemDetails:
                    DisplayMessage(async () => _dialogService.ShowToast(problemDetails.Message, ToastType.Negative));
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

        private string FormatErrorMessages(IReadOnlyList<string> errorMessages)
        {
            var result = string.Empty;

            if (errorMessages.Count == 1)
            {
                return errorMessages.SingleOrDefault();
            }

            foreach (var errorMessage in errorMessages)
            {
                result += ListMark + " " + errorMessage;
                if (errorMessage != errorMessages.Last())
                {
                    result += "\n\n";
                }
            }

            return result;
        }

        private bool TryDisplayInternalErrorMessages(IReadOnlyList<string> errorMessages)
        {
            if (errorMessages != null && errorMessages.Count > 0)
            {
                UserDialogs.Instance.Alert(FormatErrorMessages(errorMessages));
                return true;
            }

            return false;
        }

        private void DisplayMessage(Func<Task> messageAction, IReadOnlyList<string> errorMessages = null)
        {
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await _semaphore.WaitAsync(ZeroSkipDelay))
                {
                    return;
                }

                try
                {
                    if (errorMessages != null && errorMessages.Count > 0 && TryDisplayInternalErrorMessages(errorMessages))
                    {
                        return;
                    }

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
