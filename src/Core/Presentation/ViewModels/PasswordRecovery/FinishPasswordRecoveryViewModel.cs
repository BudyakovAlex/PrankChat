using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BaseViewModel
    {
        public MvxAsyncCommand ShowLoginCommand => new MvxAsyncCommand(OnShowLoginAsync);

        public MvxAsyncCommand ShowPublicationCommand => new MvxAsyncCommand(OnShowPublicationAsync);

        public FinishPasswordRecoveryViewModel(INavigationService navigationService,
                                                IErrorHandleService errorHandleService,
                                                IApiService apiService,
                                                IDialogService dialogService,
                                                ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        private Task OnShowLoginAsync()
        {
            return NavigationService.ShowLoginView();
        }

        private Task OnShowPublicationAsync()
        {
            return NavigationService.ShowMainView();
        }
    }
}
