using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class PasswordRecoveryViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand RecoverPasswordCommand => new MvxAsyncCommand(OnRecoverPassword);

        public PasswordRecoveryViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private Task OnRecoverPassword()
        {
            return NavigationService.ShowFinishPasswordRecoveryView();
        }
    }
}
