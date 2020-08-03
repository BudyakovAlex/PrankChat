using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : ExternalAuthViewModel
    {
        public RegistrationViewModel(IExternalAuthService externalAuthService, IPushNotificationService pushNotificationService)
            : base(externalAuthService, pushNotificationService)
        {
            ShowSecondStepCommand = new MvxAsyncCommand(OnShowSecondStepAsync);
            LoginCommand = new MvxAsyncCommand<LoginType>(LoginAsync);
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

        private async Task OnShowSecondStepAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            var isExists = await ApiService.CheckIsEmailExistsAsync(Email);
            if (isExists is null)
            {
                return;
            }

            if (isExists.Value)
            {
                DialogService.ShowToast(Resources.Email_Already_Exists, ToastType.Negative);
                return;
            }

            await NavigationService.ShowRegistrationSecondStepView(Email);
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Email, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "E-mail can't be empty.");
                return false;
            }

            if (!Email.IsValidEmail())
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Email, ValidationErrorType.Invalid));
                ErrorHandleService.LogError(this, "E-mail is invalid.");
                return false;
            }

            return true;
        }
    }
}
