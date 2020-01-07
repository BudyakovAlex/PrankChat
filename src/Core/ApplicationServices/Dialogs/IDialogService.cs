using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public interface IDialogService
    {
        Task<string> ShowMenuDialogAsync(string[] itemStrings, string cancelItemString = "", CancellationToken? cancellationToken = null);

        Task<DateTime?> ShowDateDialogAsync(DateTime? initialDateTime = null);

        Task ShowShareDialogAsync(string url);
    }
}
