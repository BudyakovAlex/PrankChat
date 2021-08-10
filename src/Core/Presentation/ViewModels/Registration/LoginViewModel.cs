using MvvmCross.Commands;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.Core.Services.Notifications;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : ExternalAuthViewModel
    {
        public LoginViewModel(
            IAuthorizationManager authorizationManager,
            IVersionManager versionManager,
            IUsersManager usersManager,
            IExternalAuthService externalAuthService,
            IPushNotificationProvider pushNotificationService)
            : base(authorizationManager, versionManager, usersManager, externalAuthService, pushNotificationService)
        {
#if DEBUG
            EmailText = "8118887@gmail.com";
            PasswordText = "88888888";
#endif
            LoginWithAppleCommand = this.CreateCommand<AppleAuth>(LoginWithAppleAsync);
            ShowDemoModeCommand = this.CreateCommand(NavigationManager.NavigateAsync<MainViewModel>);
            LoginCommand = this.CreateCommand<LoginType>(LoginAsync);
            ResetPasswordCommand = this.CreateCommand(NavigationManager.NavigateAsync<PasswordRecoveryViewModel>);
            RegistrationCommand = this.CreateCommand(NavigationManager.NavigateAsync<RegistrationViewModel>);
        }

        public IMvxAsyncCommand ShowDemoModeCommand { get; }
        public IMvxAsyncCommand<LoginType> LoginCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public IMvxAsyncCommand RegistrationCommand { get; }
        public IMvxAsyncCommand<AppleAuth> LoginWithAppleCommand { get; }

        private string _emailText;
        public string EmailText
        {
            get => _emailText;
            set => SetProperty(ref _emailText, value?.WithoutSpace());
        }

        private string _passwordText;
        public string PasswordText
        {
            get => _passwordText;
            set => SetProperty(ref _passwordText, value);
        }

        private async Task LoginAsync(LoginType loginType)
        {
            try
            {
                var isUpToDate = await CheckIsAppUpToDateAsync();
                if (!isUpToDate)
                {
                    return;
                }

                switch (loginType)
                {
                    case LoginType.Vk:
                    case LoginType.Facebook:
                        var isLoggedIn = await TryLoginWithExternalServicesAsync(loginType);
                        if (!isLoggedIn)
                        {
                            return;
                        }

                        break;

                    case LoginType.Ok:
                    case LoginType.Gmail:
                        return;

                    case LoginType.UsernameAndPassword:
                        if (!CheckValidation())
                        {
                            return;
                        }

                        var email = EmailText?.Trim();
                        var password = PasswordText?.Trim();
                        await AuthorizationManager.AuthorizeAsync(email, password);
                        break;
                }

                await NavigateAfterLoginAsync();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Can't sign into application.", ex);
            }
        }

        private async Task LoginWithAppleAsync(AppleAuth appleAuth)
        {
            var isUpToDate = await CheckIsAppUpToDateAsync();
            if (!isUpToDate)
            {
                return;
            }

            try
            {
                var isAuthorized = await AuthorizationManager.AuthorizeWithAppleAsync(appleAuth);
                if (!isAuthorized)
                {
                    return;
                }

                await NavigateAfterLoginAsync();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Can't sign into application.", ex);
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(EmailText))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Email, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "E-mail field can't be empty.");
                return false;
            }

            if (!EmailText.IsValidEmail())
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Email, ValidationErrorType.Invalid));
                ErrorHandleService.LogError(this, "E-mail field value is incorrect.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Password, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Password field can't be empty.");
                return false;
            }

            return true;
        }

        private async Task<bool> CheckIsAppUpToDateAsync()
        {
            var newActualVersion = await VersionManager.CheckAppVersionAsync();
            if (!string.IsNullOrEmpty(newActualVersion?.Link))
            {
                await NavigationManager.NavigateAsync<MaintananceViewModel, string>(newActualVersion?.Link);
                return false;
            }

            return true;
        }
    }
}
