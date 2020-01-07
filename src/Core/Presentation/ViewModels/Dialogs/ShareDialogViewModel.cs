using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ShareDialogViewModel : BaseViewModel, IMvxViewModel<string>
    {
        private string _url;
        private readonly IPlatformService _platformService;

        public MvxAsyncCommand ShareToInstagramCommand => new MvxAsyncCommand(OnShareToInstagramAsync);

        public MvxAsyncCommand CopyLinkCommand => new MvxAsyncCommand(OnCopyLinkAsync);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(OnShareAsync);

        public ShareDialogViewModel(INavigationService navigationService, IPlatformService platformService) : base(navigationService)
        {
            _platformService = platformService;
        }

        public void Prepare(string parameter)
        {
            _url = parameter;
        }

        private Task OnShareToInstagramAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnCopyLinkAsync()
        {
            await _platformService.CopyTextAsync(_url);
            await GoBackCommand.ExecuteAsync();
        }

        private async Task OnShareAsync()
        {
            await _platformService.ShareUrlAsync("Share publication", _url);
            await GoBackCommand.ExecuteAsync();
        }
    }
}