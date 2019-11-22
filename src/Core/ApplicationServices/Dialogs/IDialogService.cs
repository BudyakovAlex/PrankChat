using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Dialogs
{
    public interface IDialogService
    {
        Task<string> ShowFilterSelectionAsync(string[] itemStrings, CancellationToken? cancellationToken = null);
    }
}
