using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMvxLog _mvxLog;

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
                              IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _mvxLog = mvxLog;
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
                    case LoginType.Ok:
                    case LoginType.Facebook:
                    case LoginType.Gmail:
                        return;
                    case LoginType.UsernameAndPassword:
                        if (!CheckValidation())
                            return;

                        var email = EmailText?.Trim();
                        var password = PasswordText?.Trim();
                        await ApiService.AuthorizeAsync(email, password);
                        break;
                }

                // todo: not wait
                await ApiService.GetCurrentUserAsync();
                await NavigationService.ShowMainView();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Can't sign into application.", ex);
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
                ErrorHandleService.HandleException(new ValidationException(string.Empty));
                ErrorHandleService.LogError(this, "E-mail field can't be empty.");
                return false;
            }

            if (!EmailText.IsValidEmail())
            {
                ErrorHandleService.HandleException(new ValidationException(string.Empty));
                ErrorHandleService.LogError(this, "E-mail field value is incorrect.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                ErrorHandleService.HandleException(new ValidationException(string.Empty));
                ErrorHandleService.LogError(this, "Password field can't be empty.");
                return false;
            }

            return true;
        }
    }
}
