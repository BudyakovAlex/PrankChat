using System;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class WebViewModel : BaseViewModel, IMvxViewModel<WebViewNavigationParameter>
    {
        public string Url { get; set; }

        public WebViewModel(INavigationService navigationService,
                            IErrorHandleService errorHandleService,
                            IApiService apiService,
                            IDialogService dialogService,
                            ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public void Prepare(WebViewNavigationParameter parameter)
        {
            Url = parameter.Url;
        }
    }
}
