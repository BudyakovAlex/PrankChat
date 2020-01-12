using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IDialogService _dialogService;

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

        public LoginViewModel(INavigationService navigationService, IApiService apiService, IDialogService dialogService) : base(navigationService)
        {
            _apiService = apiService;
            _dialogService = dialogService;

            EmailText = "formation7@outlook.com";
            PasswordText = "qweqweqwe";
        }

        public MvxAsyncCommand<string> LoginCommand => new MvxAsyncCommand<string>(OnLoginCommand);

        public MvxAsyncCommand ResetPasswordCommand => new MvxAsyncCommand(() => NavigationService.ShowPasswordRecoveryView());

        public MvxAsyncCommand RegistrationCommand => new MvxAsyncCommand(() => NavigationService.ShowRegistrationView());

        private async Task OnLoginCommand(string loginType)
        {
            if (Enum.TryParse<LoginType>(loginType, out var socialNetworkType))
            {
                switch (socialNetworkType)
                {
                    case LoginType.Vk:
                        break;

                    case LoginType.Ok:
                        break;

                    case LoginType.Facebook:
                        break;

                    case LoginType.Gmail:
                        break;

                    case LoginType.UsernameAndPassword:
                        var email = EmailText?.Trim();
                        var password = PasswordText?.Trim();
                        await _apiService.AuthorizeAsync(email, password);
                        break;
                }
            }
            else
            {
                _dialogService.ShowToastAsync("Error with login type!");
            }

            await NavigationService.ShowMainView();
        }
    }
}
