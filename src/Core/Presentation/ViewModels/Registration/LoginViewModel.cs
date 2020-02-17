using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMvxLog _mvxLog;
        private readonly ISettingsService _settingsService;

        private string _emailText;
        public string EmailText
        {
            get => _emailText;
            set => SetProperty(ref _emailText, value);
        }

        private string _passwordText;
        public string PasswordText
        {
            get => _passwordText;
            set => SetProperty(ref _passwordText, value);
        }

        public LoginViewModel(INavigationService navigationService,
                              IApiService apiService,
                              IDialogService dialogService,
                              IMvxLog mvxLog,
                              IErrorHandleService errorHandleService,
                              ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _mvxLog = mvxLog;
            _settingsService = settingsService;
#if DEBUG

            EmailText = "testuser@delete.me";
            PasswordText = "123456789";
#endif
        }

        public MvxAsyncCommand<string> LoginCommand => new MvxAsyncCommand<string>(OnLoginCommand);

        public MvxAsyncCommand ResetPasswordCommand => new MvxAsyncCommand(() => NavigationService.ShowPasswordRecoveryView());

        public MvxAsyncCommand RegistrationCommand => new MvxAsyncCommand(() => NavigationService.ShowRegistrationView());

        private async Task OnLoginCommand(string loginType)
        {
            try
            {
                IsBusy = true;

                if (!Enum.TryParse<LoginType>(loginType, out var socialNetworkType))
                {
                    throw new ArgumentException(nameof(loginType));
                }

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
                        if (!CheckValidation())
                            return;

                        var email = EmailText?.Trim();
                        var password = PasswordText?.Trim();
                        await ApiService.AuthorizeAsync(email, password);
                        break;
                }

                // todo: not wait
               if (!await ApiService.GetCurrentUserAsync())
                {
                    throw new Exception("Error with login. Unable to get current user");
                }
                
                await NavigationService.ShowMainView();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(new UserVisibleException("Проблема с входом в приложение. Попробуйте еще раз."));
                _mvxLog.ErrorException($"[{nameof(LoginViewModel)}]", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(EmailText))
            {
                ErrorHandleService.HandleException(new UserVisibleException("Email не может быть пустым."));
                return false;
            }

            if (!EmailText.IsValidEmail())
            {
                ErrorHandleService.HandleException(new UserVisibleException("Поле Email введено не правильно."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                ErrorHandleService.HandleException(new UserVisibleException("Пароль не может быть пустым."));
                return false;
            }

            return true;
        }
    }
}
