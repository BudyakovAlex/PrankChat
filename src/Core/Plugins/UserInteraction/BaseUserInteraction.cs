using Acr.UserDialogs;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Results;
using PrankChat.Mobile.Core.ViewModels.Dialogs;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Plugins.UserInteraction
{
    public abstract class BaseUserInteraction : IUserInteraction
    {
        private readonly INavigationManager _navigationManager;

        protected BaseUserInteraction(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public abstract bool IsToastShown { get; protected set; }

        public abstract Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null);

        public async Task<string> ShowMenuDialogAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null)
        {
            if (CrossDeviceInfo.Current.Platform == Platform.iOS)
            {
                var cancelText = string.IsNullOrWhiteSpace(cancelItemString) ? Resources.Cancel : cancelItemString;
                var result = await UserDialogs.Instance.ActionSheetAsync(null, cancelText, null, cancellationToken, itemStrings);
                if (result == cancelText)
                {
                    return null;
                }

                return result;
            }

            return await UserDialogs.Instance.ActionSheetAsync(null, cancelItemString, null, cancellationToken, itemStrings);
        }

        public Task ShowShareDialogAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            return _navigationManager.NavigateAsync<ShareDialogViewModel, ShareDialogParameter>(new ShareDialogParameter(url));
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
            var result = await _navigationManager.NavigateAsync<ArrayDialogViewModel, ArrayDialogParameter, ArrayDialogResult>(new ArrayDialogParameter(items, title));
            return result?.SelectedItem;
        }

        public abstract void ShowToast(string text, ToastType toastType);
    }
}