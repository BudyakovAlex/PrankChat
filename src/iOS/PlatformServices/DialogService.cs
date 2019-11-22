using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.iOS.PlatformServices
{
    public class DialogService : IDialogService
    {
        public Task<string> ShowFilterSelectionAsync(string[] itemStrings, CancellationToken? cancellationToken = null)
        {
            return UserDialogs.Instance.ActionSheetAsync(null, Resources.Cancel, null, cancellationToken, itemStrings);
        }
    }
}
