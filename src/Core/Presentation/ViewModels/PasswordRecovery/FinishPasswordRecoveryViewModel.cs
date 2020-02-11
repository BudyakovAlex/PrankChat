using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BaseViewModel
    {
        public MvxAsyncCommand FinishRecoveringPasswordCommand => new MvxAsyncCommand(OnFinishRecoverPassword);

        public FinishPasswordRecoveryViewModel(INavigationService navigationService,
                                                IErrorHandleService errorHandleService,
                                                IApiService apiService,
                                                IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }

        private Task OnFinishRecoverPassword()
        {
            return NavigationService.ShowMainView();
        }
    }
}
