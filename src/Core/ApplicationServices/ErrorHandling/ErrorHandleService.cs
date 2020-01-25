using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Presentation.Localization;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.ErrorHandling
{
    public class ErrorHandleService : IErrorHandleService
    {
        private const string ListMark = "-";
        private const int ZeroSkipDelay = 0;

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IMvxMessenger _messenger;
        private readonly IDialogService _dialogService;

        public ErrorHandleService(IMvxMessenger messenger, IDialogService dialogService)
        {
            _messenger = messenger;
            _dialogService = dialogService;

            _messenger.Subscribe<BadRequestErrorMessage>(OnBadRequestErrorEvent, MvxReference.Strong);
            _messenger.Subscribe<NotFoundErrorMessage>(OnNotFoundErrorEvent, MvxReference.Strong);
            _messenger.Subscribe<ServerErrorMessage>(OnServerErrorEvent, MvxReference.Strong);
        }

        public void HandleException(Exception exception)
        {
            switch (exception)
            {
                case UserVisibleException _:
                    DisplayMessage(async () => _dialogService.ShowToast(exception.Message));
                    break;
            }
        }

        private void OnNotFoundErrorEvent(NotFoundErrorMessage e)
        {
            DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Not_Found), e.ErrorMessages);
        }

        private void OnBadRequestErrorEvent(BadRequestErrorMessage e)
        {
            DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Network), e.ErrorMessages);
        }

        private void OnServerErrorEvent(ServerErrorMessage e)
        {
            DisplayMessage(async () => await _dialogService.ShowAlertAsync(Resources.Error_Unexpected_Server));
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
