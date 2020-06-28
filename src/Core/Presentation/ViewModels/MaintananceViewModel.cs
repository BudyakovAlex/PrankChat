using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MaintananceViewModel : BaseViewModel, IMvxViewModel<string>
    {
        private string _applicationUrl;

        public MaintananceViewModel(INavigationService navigationService, IErrorHandleService errorHandleService,
                                    IApiService apiService, IDialogService dialogService,
                                    ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            OpenInBrowserCommand = new MvxAsyncCommand(OpenInBrowserAsync);
        }

        public IMvxAsyncCommand OpenInBrowserCommand { get; }

        public void Prepare(string parameter)
        {
            _applicationUrl = parameter;
        }

        private Task OpenInBrowserAsync()
        {
            return Browser.OpenAsync(_applicationUrl, BrowserLaunchMode.External);
        }
    }
}