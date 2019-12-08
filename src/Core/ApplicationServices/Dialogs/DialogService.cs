using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public class DialogService : IDialogService
    {
        public Task<string> ShowFilterSelectionAsync(string[] itemStrings, string cancelItemString = null, CancellationToken? cancellationToken = null)
        {
            var cancelText = cancelItemString ?? string.Empty;

            if (CrossDeviceInfo.Current.Platform == Platform.iOS)
            {
                return UserDialogs.Instance.ActionSheetAsync(null, cancelText, null, cancellationToken, itemStrings);
            }
            return UserDialogs.Instance.ActionSheetAsync(null, cancelText, null, cancellationToken, itemStrings);
        }
    }
}
