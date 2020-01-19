using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public interface IDialogService
    {
        Task<string> ShowMenuDialogAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null);

        Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null);

        void ShowToast(string text);
        
        Task ShowShareDialogAsync(string url);

        Task<bool> ShowConfirmAsync(string message, string title = "", string ok = "", string cancel = "");
    }
}
