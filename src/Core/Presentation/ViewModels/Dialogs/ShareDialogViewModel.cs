using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs
{
    public class ShareDialogViewModel : BaseViewModel, IMvxViewModel<ShareDialogParameter>
    {
        private readonly IPlatformService _platformService;

        private string _url;

        public MvxAsyncCommand ShareToInstagramCommand => new MvxAsyncCommand(OnShareToInstagramAsync);

        public MvxAsyncCommand CopyLinkCommand => new MvxAsyncCommand(OnCopyLinkAsync);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(OnShareAsync);

        public ShareDialogViewModel(INavigationService navigationService,
                                    IPlatformService platformService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _platformService = platformService;
        }

        public void Prepare(ShareDialogParameter parameter)
        {
            _url = parameter.Url;
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
            await _platformService.ShareUrlAsync(Resources.ShareDialog_LinkShareTitle, _url);
            if(CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                await NavigationService.CloseView(this);
            }
        }
    }
}