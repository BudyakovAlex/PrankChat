﻿using System;
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
        public async Task<DateTime?> ShowDateDialogAsync()
        {
            var result = await UserDialogs.Instance.DatePromptAsync();
            if (result.Ok) {
                return result.SelectedDate;
            }

            return null;
        }

        public Task<string> ShowFilterSelectionAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null)
        {
            if (CrossDeviceInfo.Current.Platform == Platform.iOS)
            {
                var cancelText = string.IsNullOrWhiteSpace(cancelItemString) ? Resources.Cancel : cancelItemString;
                return UserDialogs.Instance.ActionSheetAsync(null, cancelText, null, cancellationToken, itemStrings);
            }
            return UserDialogs.Instance.ActionSheetAsync(null, cancelItemString, null, cancellationToken, itemStrings);
        }
    }
}
