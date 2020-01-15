using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
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
            set
            {
                SetProperty(ref _birthday, value);

            }
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

        private GenderType _gender;
        public GenderType Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<string> SelectGenderCommand => new MvxCommand<string>(OnSelectGender);

        public MvxAsyncCommand UserRegistrationCommand => new MvxAsyncCommand(OnUserRegistrationAsync);

        public RegistrationSecondStepViewModel(INavigationService navigationService,
                                                IDialogService dialogService,
                                                IApiService apiService,
                                                IMvxLog mvxLog)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
        }

        public void Prepare(RegistrationNavigationParameter parameter)
        {
            _email = parameter.Email;

#if DEBUG

            _email = "formation7@outlook.com";
            Nickname = "formation7";
            Name = "test user";
            Birthday = new DateTime(1992,4,11);
            Password = "qweqweqwe";
            RepeatedPassword = "qweqweqwe";
            Gender = GenderType.Male;
#endif
        }

        private void OnSelectGender(string genderTypeString)
        {
            if (Enum.TryParse<GenderType>(genderTypeString, out var genderType))
            {
                Gender = genderType;
            }
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
            try
            {
                IsBusy = true;

                var userInfo = new UserRegistrationDataModel()
                {
                    Name = Name,
                    Email = _email,
                    Login = Nickname,
                    Birthday = Birthday.Value,
                    Password = Password,
                    PasswordConfirmation = RepeatedPassword,
                };
                await _apiService.RegisterAsync(userInfo);
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
    }
}
