using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;

namespace PrankChat.Mobile.Droid.PlatformServices
{
    public class DialogService : IDialogService
    {
        public Task<string> ShowFilterSelectionAsync(string[] itemStrings, CancellationToken? cancellationToken = null)
        {
            return UserDialogs.Instance.ActionSheetAsync(null, string.Empty, null, cancellationToken, buttons: itemStrings);
        }
    }
}
