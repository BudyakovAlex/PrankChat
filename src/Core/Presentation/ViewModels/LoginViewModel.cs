using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public MvxAsyncCommand LoginCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowMainView());
            }
        }

        public MvxAsyncCommand ResetPasswordCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowPasswordRecoveryView());
            }
        }

        public MvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationView());
            }
        }

        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
