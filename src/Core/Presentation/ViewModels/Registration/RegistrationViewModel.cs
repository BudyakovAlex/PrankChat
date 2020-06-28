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
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand ShowSecondStepCommand => new MvxAsyncCommand(OnShowSecondStepAsync);

        public MvxAsyncCommand<LoginType> LoginCommand => new MvxAsyncCommand<LoginType>(LoginAsync);

        public RegistrationViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IApiService apiService,
                                     IErrorHandleService errorHandleService,
                                     ISettingsService settingsService,
                                     IExternalAuthService externalAuthService,
                                     IPushNotificationService pushNotificationService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService, externalAuthService, pushNotificationService)
        {
        }

        private async Task LoginAsync(LoginType loginType)
        {
            var result = await TryLoginWithExternalServicesAsync(loginType);
            if (!result)
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
            if (isExists)
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
