﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Navigation.Arguments.NavigationParameters;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Services.Notifications;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Profile.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseProfileViewModel, IMvxViewModel<GenericNavigationParams<RegistrationNavigationParameter>>
    {
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IPushNotificationProvider _pushNotificationService;

        public RegistrationSecondStepViewModel(
            IAuthorizationManager authorizationManager,
            IUsersManager usersManager,
            IPushNotificationProvider pushNotificationService) : base(usersManager)
        {
            _authorizationManager = authorizationManager;
            _pushNotificationService = pushNotificationService;

            UserRegistrationCommand = this.CreateCommand(UserRegistrationAsync);
            ShowTermsAndRulesCommand = this.CreateCommand(ShowTermsAndRulesAsync);
        }

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

        private bool _isPolicyChecked;
        public bool IsPolicyChecked
        {
            get => _isPolicyChecked;
            set => SetProperty(ref _isPolicyChecked, value);
        }

        private bool _isAdultChecked;
        public bool IsAdultChecked
        {
            get => _isAdultChecked;
            set => SetProperty(ref _isAdultChecked, value);
        }

        public IMvxAsyncCommand UserRegistrationCommand { get; }

        public IMvxCommand ShowTermsAndRulesCommand { get; }

        public void Prepare(GenericNavigationParams<RegistrationNavigationParameter> parameter)
        {
            Email = parameter.Parameter.Email;
        }

        private Task ShowTermsAndRulesAsync() =>
            Xamarin.Essentials.Browser.OpenAsync(RestConstants.PolicyEndpoint);

        private async Task UserRegistrationAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            try
            {
                var userInfo = new UserRegistration(
                    Name,
                    Email,
                    Login,
                    Birthday,
                    Gender,
                    Password,
                    RepeatedPassword);

                await _authorizationManager.RegisterAsync(userInfo);
                // TODO: not wait
                await UsersManager.GetAndRefreshUserInSessionAsync();

                await _pushNotificationService.TryUpdateTokenAsync();
                await NavigationManager.NavigateAsync<RegistrationThirdStepViewModel>();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "User registration error occured.", ex);
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Login, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Login can't be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Name, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Name can't be empty.");
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Birthday, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Birthday can't be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Password, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Password can't be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(RepeatedPassword))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.PasswordRepeat, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Password repeat can't be empty.");
                return false;
            }

            if (Password != RepeatedPassword)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Password, ValidationErrorType.NotMatch, Resources.PasswordRepeat));
                ErrorHandleService.LogError(this, "Password and repeated password values don't match.");
                return false;
            }

            if (!IsAdultChecked)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.YouAreNotConfirmedAdultAge));
                ErrorHandleService.LogError(this, "Adult not checked");
                return false;
            }

            if (!IsPolicyChecked)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.YouAreNotConfirmedUserAgreement));
                ErrorHandleService.LogError(this, "Policy not checked");
                return false;
            }

            return true;
        }
    }
}
