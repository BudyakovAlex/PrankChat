using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class LoginViewModel : ExternalAuthViewModel
    {
        public LoginViewModel(IExternalAuthService externalAuthService, IPushNotificationService pushNotificationService)
            : base(externalAuthService, pushNotificationService)
        {
#if DEBUG

            EmailText = "alexeysorochan@gmail.com";
            PasswordText = "qqqqqqqq";
#endif
            LoginWithAppleCommand = new MvxAsyncCommand<AppleAuthDataModel>(LoginWithAppleAsync);
            ShowDemoModeCommand = new MvxAsyncCommand(() => NavigationService.ShowMainView());
            LoginCommand = new MvxAsyncCommand<string>(LoginAsync);
            ResetPasswordCommand = new MvxAsyncCommand(() => NavigationService.ShowPasswordRecoveryView());
            RegistrationCommand = new MvxAsyncCommand(() => NavigationService.ShowRegistrationView());
    }

        private string _emailText;
        public string EmailText
        {
            get => _emailText;
            set
            {
                _emailText = value?.WithoutSpace();
                RaisePropertyChanged(nameof(EmailText));
            }
        }

        private string _passwordText;
        public string PasswordText
        {
            get => _passwordText;
            set => SetProperty(ref _passwordText, value);
        }

        public IMvxAsyncCommand ShowDemoModeCommand { get; }

        public IMvxAsyncCommand<string> LoginCommand { get; }

        public IMvxAsyncCommand ResetPasswordCommand { get; }

        public IMvxAsyncCommand RegistrationCommand { get; }

        public IMvxAsyncCommand<AppleAuthDataModel> LoginWithAppleCommand { get; }

        private Task LoginAsync(string loginType)
        {
            return ExecutionStateWrapper.WrapAsync(async () =>
            {
                try
                {
                    if (!SettingsService.IsDebugMode)
                    {
                        var newActualVersion = await ApiService.CheckAppVersionAsync();
                        if (!string.IsNullOrEmpty(newActualVersion?.Link))
                        {
                            await NavigationService.ShowMaintananceView(newActualVersion.Link);
                            return;
                        }
                    }
                    if (!Enum.TryParse<LoginType>(loginType, out var socialNetworkType))
                    {
                        throw new ArgumentException(nameof(loginType));
                    }

                    switch (socialNetworkType)
                    {
                        case LoginType.Vk:
                        case LoginType.Facebook:
                            var isLoggedIn = await TryLoginWithExternalServicesAsync(socialNetworkType);
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
                            await ApiService.AuthorizeAsync(email, password);
                            break;
                    }

                    await NavigateAfterLoginAsync();
                }
                catch (Exception ex)
                {
                    ErrorHandleService.HandleException(ex);
                    ErrorHandleService.LogError(this, "Can't sign into application.", ex);
                }
            });
        }

        private Task LoginWithAppleAsync(AppleAuthDataModel appleAuthDataModel)
        {
            return ExecutionStateWrapper.WrapAsync(async () =>
            {
                if (!SettingsService.IsDebugMode)
                {
                    var newActualVersion = await ApiService.CheckAppVersionAsync();
                    if (!string.IsNullOrEmpty(newActualVersion?.Link))
                    {
                        await NavigationService.ShowMaintananceView(newActualVersion.Link);
                        return;
                    }
                }

                try
                {
                    var isAuthorized = await ApiService.AuthorizeWithAppleAsync(appleAuthDataModel);
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
            });
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
    }
}
