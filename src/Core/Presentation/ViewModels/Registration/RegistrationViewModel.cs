using MvvmCross.Commands;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.Core.Services.Notifications;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : ExternalAuthViewModel
    {
        public RegistrationViewModel(
            IAuthorizationManager authorizationManager,
            IVersionManager versionManager,
            IUsersManager usersManager,
            IExternalAuthService externalAuthService,
            IPushNotificationProvider pushNotificationService) : base(authorizationManager, versionManager, usersManager, externalAuthService, pushNotificationService)
        {
            ShowSecondStepCommand = this.CreateCommand(ShowSecondStepAsync);
            LoginCommand = this.CreateCommand<LoginType>(LoginAsync);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public IMvxAsyncCommand ShowSecondStepCommand { get; }

        public IMvxAsyncCommand<LoginType> LoginCommand { get; }

        private async Task LoginAsync(LoginType loginType)
        {
            var isLoggedIn = await TryLoginWithExternalServicesAsync(loginType);
            if (!isLoggedIn)
            {
                return;
            }

            await NavigateAfterLoginAsync();
        }

        private async Task ShowSecondStepAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            var isExists = await AuthorizationManager.CheckIsEmailExistsAsync(Email);
            if (isExists is null || isExists.Value)
            {
                UserInteraction.ShowToast(Resources.EmailAlreadyExists, ToastType.Negative);
                return;
            }

            var parameter = new RegistrationNavigationParameter(Email);
            await NavigationManager.NavigateAsync<RegistrationSecondStepViewModel, RegistrationNavigationParameter>(parameter);
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Email, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "E-mail can't be empty.");
                return false;
            }

            if (!Email.IsValidEmail())
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Email, ValidationErrorType.Invalid));
                ErrorHandleService.LogError(this, "E-mail is invalid.");
                return false;
            }

            return true;
        }
    }
}
