using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ShareDialogViewModel : BasePageViewModel, IMvxViewModel<ShareDialogParameter>
    {
        private string _url;

        public ShareDialogViewModel()
        {
            ShareToInstagramCommand = this.CreateCommand(ShareToInstagramAsync);
            CopyLinkCommand = this.CreateCommand(CopyLinkAsync);
            ShareCommand = this.CreateCommand(ShareAsync);
        }

        public MvxAsyncCommand ShareToInstagramCommand { get; }

        public MvxAsyncCommand CopyLinkCommand { get; } 

        public MvxAsyncCommand ShareCommand { get; }

        public void Prepare(ShareDialogParameter parameter)
        {
            _url = parameter.Url;
        }

        private Task ShareToInstagramAsync()
        {
            return Task.CompletedTask;
        }

        private async Task CopyLinkAsync()
        {
            await Clipboard.SetTextAsync(_url);
            CloseCommand.Execute(null);
        }

        private async Task ShareAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = _url,
                Title = Resources.ShareLink
            });

            if (CrossDeviceInfo.Current.Platform != Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                return;
            }

            await NavigationManager.CloseAsync(this);
        }
    }
}