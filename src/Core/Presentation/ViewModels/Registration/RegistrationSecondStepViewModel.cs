using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseViewModel, IMvxViewModel<RegistrationNavigationParameter>
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IMvxLog _mvxLog;
        private readonly IErrorHandleService _errorHandleService;

        private string _email;

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set => SetProperty(ref _nickname, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime? _birthday;
        public DateTime? Birthday
        {
            get => _birthday;
            set => SetProperty(ref _birthday, value);
        }

        public string BirthdayText => Birthday?.ToShortDateString() ?? Resources.RegistrationView_Birthday_Placeholder;

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

        private GenderType? _gender;
        public GenderType? Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<GenderType> SelectGenderCommand => new MvxCommand<GenderType>(OnSelectGender);

        public MvxAsyncCommand UserRegistrationCommand => new MvxAsyncCommand(OnUserRegistrationAsync);

        public RegistrationSecondStepViewModel(INavigationService navigationService,
                                               IDialogService dialogService,
                                               IApiService apiService,
                                               IMvxLog mvxLog,
                                               IErrorHandleService errorHandleService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
            _errorHandleService = errorHandleService;
        }

        public void Prepare(RegistrationNavigationParameter parameter)
        {
            _email = parameter.Email;
        }

        private void OnSelectGender(GenderType genderType)
        {
            Gender = genderType;
        }

        private async Task OnSelectBirthdayAsync()
        {
            var result = await _dialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                Birthday = result.Value;
                await RaisePropertyChanged(nameof(BirthdayText));
            }
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
                    Email = _email,
                    Login = Nickname,
                    Birthday = Birthday,
                    Sex = Gender,
                    Password = Password,
                    PasswordConfirmation = RepeatedPassword,
                };
                await _apiService.RegisterAsync(userInfo);
                // todo: not wait
                await _apiService.GetCurrentUserAsync();
                await NavigationService.ShowRegistrationThirdStepView();
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"Exception with registration {ex.Message}");
                _mvxLog.ErrorException($"[{nameof(RegistrationSecondStepViewModel)}]", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Nickname))
            {
                _errorHandleService.HandleException(new UserVisibleException("Логин не может быть пустым."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                _errorHandleService.HandleException(new UserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (Birthday == null)
            {
                _errorHandleService.HandleException(new UserVisibleException("День рождения не может быть пустым."));
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= 18)
            {
                _errorHandleService.HandleException(new UserVisibleException("Пользователь не может быть младше 18 лет."));
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                _errorHandleService.HandleException(new UserVisibleException("Пароль не может быть пустым."));
                return false;
            }

            if (string.IsNullOrEmpty(RepeatedPassword))
            {
                _errorHandleService.HandleException(new UserVisibleException("Проверочный пароль не может быть пустым."));
                return false;
            }

            if (Password != RepeatedPassword)
            {
                _errorHandleService.HandleException(new UserVisibleException("Проверочный пароль и пароль не совпадают."));
                return false;
            }

            if (Gender == null)
            {
                _errorHandleService.HandleException(new UserVisibleException("Выберите свой пол."));
                return false;
            }

            return true;
        }
    }
}
