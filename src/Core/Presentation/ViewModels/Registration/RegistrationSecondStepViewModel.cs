using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseProfileViewModel, IMvxViewModel<RegistrationNavigationParameter>
    {
        private readonly IPushNotificationService _pushNotificationService;

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _repeatedPassword;
        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set => SetProperty(ref _repeatedPassword, value);
        }

        public MvxAsyncCommand UserRegistrationCommand => new MvxAsyncCommand(OnUserRegistrationAsync);

        public RegistrationSecondStepViewModel(INavigationService navigationService,
                                               IDialogService dialogService,
                                               IApiService apiService,
                                               IErrorHandleService errorHandleService,
                                               ISettingsService settingsService,
                                               IPushNotificationService pushNotificationService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _pushNotificationService = pushNotificationService;
        }

        public void Prepare(RegistrationNavigationParameter parameter)
        {
            Email = parameter.Email;
        }

        private async Task OnUserRegistrationAsync()
        {
            if (!CheckValidation())
                return;

            try
            {
                IsBusy = true;

                var userInfo = new UserRegistrationDataModel()
                {
                    Name = Name,
                    Email = Email,
                    Login = Login,
                    Birthday = Birthday,
                    Sex = Gender,
                    Password = Password,
                    PasswordConfirmation = RepeatedPassword,
                };
                await ApiService.RegisterAsync(userInfo);
                // todo: not wait
                await ApiService.GetCurrentUserAsync();
                await _pushNotificationService.TryUpdateTokenAsync();
                await NavigationService.ShowRegistrationThirdStepView();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "User registration error occured.", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Login, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Login can't be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Name, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Name can't be empty.");
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Birthday, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Birthday can't be empty.");
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= Constants.Age.AdultAge)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Birthday, ValidationErrorType.LowerThanRequired, Constants.Age.AdultAge.ToString()));
                ErrorHandleService.LogError(this, $"User can't be younger than {Constants.Age.AdultAge} years.");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Password, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Password can't be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(RepeatedPassword))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_PasswordRepeat, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Password repeat can't be empty.");
                return false;
            }

            if (Password != RepeatedPassword)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Password, ValidationErrorType.NotMatch, Resources.Validation_Field_PasswordRepeat));
                ErrorHandleService.LogError(this, "Password and repeated password values don't match.");
                return false;
            }

            if (Gender == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Gender, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Gender can't be empty.");
                return false;
            }

            return true;
        }
    }
}
