using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
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

        public LoginViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;

            EmailText = "test2@mail.ru";
            PasswordText = "asd123456789";
        }

        public MvxAsyncCommand<string> LoginCommand => new MvxAsyncCommand<string>(OnLoginCommand);

        public MvxAsyncCommand ResetPasswordCommand => new MvxAsyncCommand(() => NavigationService.ShowPasswordRecoveryView());

        public MvxAsyncCommand RegistrationCommand => new MvxAsyncCommand(() => NavigationService.ShowRegistrationView());

        private async Task OnLoginCommand(string loginType)
        {
            if (Enum.TryParse<SocialNetworkType>(loginType, out var socialNetworkType))
            {
                var email = EmailText?.Trim();
                var password = PasswordText?.Trim();
                await _apiService.AuthorizeAsync(email, password);
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
