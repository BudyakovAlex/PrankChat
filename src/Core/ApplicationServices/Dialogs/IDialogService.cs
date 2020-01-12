﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public interface IDialogService
    {
        Task<string> ShowFilterSelectionAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null);

        Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null);

        void ShowToast(string text);
    }
}
