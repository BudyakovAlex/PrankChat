using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ShareDialogViewModel : BasePageViewModel, IMvxViewModel<ShareDialogParameter>
    {
        private readonly IPlatformService _platformService;

        private string _url;

        public MvxAsyncCommand ShareToInstagramCommand => new MvxAsyncCommand(ShareToInstagramAsync);

        public MvxAsyncCommand CopyLinkCommand => new MvxAsyncCommand(CopyLinkAsync);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(ShareAsync);

        public ShareDialogViewModel(IPlatformService platformService)
        {
            _platformService = platformService;
        }

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
            await _platformService.CopyTextAsync(_url);
            await GoBackCommand.ExecuteAsync();
        }

        private async Task ShareAsync()
        {
            await _platformService.ShareUrlAsync(Resources.ShareDialog_LinkShareTitle, _url);
            if (CrossDeviceInfo.Current.Platform != Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                return;
            }

            await NavigationManager.CloseAsync(this);
        }
    }
}