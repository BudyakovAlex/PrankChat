using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseViewModel
    {
        private IDialogService _dialogService;

        public RegistrationSecondStepViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

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

        public MvxAsyncCommand ShowThirdStepCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationThirdStepView());
            }
        }

        private GenderType _gender;

        public GenderType Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthday);

        public MvxCommand<string> SelectGenderCommand => new MvxCommand<string>(OnSelectGender);

        private void OnSelectGender(string genderTypeString)
        {
            if (Enum.TryParse<GenderType>(genderTypeString, out var genderType))
            {
                Gender = genderType;
            }
        }

        private async Task OnSelectBirthday()
        {
            var result = await _dialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                Birthday = result.Value;
                await RaisePropertyChanged(nameof(BirthdayText));
            }
        }
    }
}
