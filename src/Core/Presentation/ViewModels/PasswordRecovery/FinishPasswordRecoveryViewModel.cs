using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class FinishPasswordRecoveryViewModel : BaseViewModel
    {
        public MvxAsyncCommand FinishRecoveringPasswordCommand => new MvxAsyncCommand(OnFinishRecoverPassword);

        public FinishPasswordRecoveryViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private Task OnFinishRecoverPassword()
        {
            return NavigationService.ShowMainView();
        }
    }
}
