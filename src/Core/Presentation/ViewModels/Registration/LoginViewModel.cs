using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : BaseViewModel
    {
        private string _emailText;
        private string _passwordText;

        public string EmailText
        {
            get => _emailText;
            set => SetProperty(ref _emailText, value);
        }

        public string PasswordText
        {
            get => _passwordText;
            set => SetProperty(ref _passwordText, value);
        }

        public MvxAsyncCommand<string> LoginCommand
        {
            get
            {
                return new MvxAsyncCommand<string>(OnLoginCommand);
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

        private async Task OnLoginCommand(string loginType)
        {
            if (Enum.TryParse<SocialNetworkType>(loginType, out var socialNetworkType))
            {
                await NavigationService.ShowMainView();

                switch (socialNetworkType)
                {
                    case SocialNetworkType.Vk:
                        break;

                    case SocialNetworkType.Ok:
                        break;

                    case SocialNetworkType.Facebook:
                        break;

                    case SocialNetworkType.Gmail:
                        break;

                    default:
                        break;
                }
            }
            else
            {
                await NavigationService.ShowMainView();
            }
        }
    }
}
