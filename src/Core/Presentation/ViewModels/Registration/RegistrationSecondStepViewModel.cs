﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
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
        private readonly IMvxLog _mvxLog;

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
                                               IMvxLog mvxLog,
                                               IErrorHandleService errorHandleService,
                                               ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxLog = mvxLog;
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
                await NavigationService.ShowRegistrationThirdStepView();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Проблема с регистрацией пользователя."));
                _mvxLog.ErrorException($"[{nameof(RegistrationSecondStepViewModel)}]", ex);
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
                ErrorHandleService.HandleException(new BaseUserVisibleException("Логин не может быть пустым."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("День рождения не может быть пустым."));
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= 18)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Пользователь не может быть младше 18 лет."));
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Пароль не может быть пустым."));
                return false;
            }

            if (string.IsNullOrEmpty(RepeatedPassword))
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Проверочный пароль не может быть пустым."));
                return false;
            }

            if (Password != RepeatedPassword)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Проверочный пароль и пароль не совпадают."));
                return false;
            }

            if (Gender == null)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Выберите свой пол."));
                return false;
            }

            return true;
        }
    }
}
