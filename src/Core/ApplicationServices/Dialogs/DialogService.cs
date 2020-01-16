using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Navigation;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public class DialogService : IDialogService
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public DialogService(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
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

            return _mvxNavigationService.Navigate<ShareDialogViewModel, string>(url);
        }
    }
}
