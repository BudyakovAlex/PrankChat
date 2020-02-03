using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Navigation;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public class DialogService : IDialogService
    {
        private readonly INavigationService _navigationService;

        public DialogService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null)
        {
            var selectedDateTime = initialDateTime.HasValue ? initialDateTime : DateTime.Now;

            var result = await UserDialogs.Instance.DatePromptAsync(selectedDate: selectedDateTime);
            if (result.Ok) {
                return result.SelectedDate;
            }

            return null;
        }

        public Task<string> ShowMenuDialogAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null)
        {
            if (CrossDeviceInfo.Current.Platform == Platform.iOS)
            {
                var cancelText = string.IsNullOrWhiteSpace(cancelItemString) ? Resources.Cancel : cancelItemString;
                return UserDialogs.Instance.ActionSheetAsync(null, cancelText, null, cancellationToken, itemStrings);
            }
            return UserDialogs.Instance.ActionSheetAsync(null, cancelItemString, null, cancellationToken, itemStrings);
        }

        public void ShowToast(string text)
        {
            UserDialogs.Instance.Toast(text);
        }

        public Task ShowShareDialogAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
               throw new ArgumentNullException(nameof(url));

            return _navigationService.ShowShareDialog(new ShareDialogParameter(url));
        }

        public Task<bool> ShowConfirmAsync(string message, string title = "", string ok = "", string cancel = "")
        {
            if (string.IsNullOrWhiteSpace(ok))
            {
                ok = Resources.Ok;
            }

            if (string.IsNullOrWhiteSpace(cancel))
            {
                cancel = Resources.Cancel;
            }

            return UserDialogs.Instance.ConfirmAsync(message, title, ok, cancel);
        }

        public Task ShowAlertAsync(string message, string title = "", string ok = "")
        {
            if (string.IsNullOrWhiteSpace(ok))
            {
                ok = Resources.Ok;
            }

            return UserDialogs.Instance.AlertAsync(message, title, ok);
        }

        public async Task<string> ShowArrayDialogAsync(List<string> items, string title = "")
        {
            var result = await _navigationService.ShowArrayDialog(new ArrayDialogParameter(items, title));
            return result?.SelectedItem;
        }
    }
}
