using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileUpdateViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;

        private string _email;
        private string _login;
        private string _name;
        private DateTime? _birthdate;
        private GenderType? _gender;


        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DateTime? Birthday
        {
            get => _birthdate;
            set => SetProperty(ref _birthdate, value);
        }

        public string BirthdayText => Birthday?.ToShortDateString() ?? Resources.ProfileUpdateView_Birthday_Placeholder;

        public GenderType? Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<GenderType> SelectGenderCommand => new MvxCommand<GenderType>(OnSelectGender);

        public MvxAsyncCommand ProfileUpdateCommand => new MvxAsyncCommand(OnProfileUpdateAsync);

        public MvxAsyncCommand ChangePasswordCommand => new MvxAsyncCommand(OnChangePasswordAsync);

        public ProfileUpdateViewModel(INavigationService navigationService,
                                      ISettingsService settingsService,
                                      IDialogService dialogService,
                                      IApiService apiService) : base(navigationService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _apiService = apiService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            var user = _settingsService.User;

            if (user == null)
                return;

            Email = user.Email;
            Name = user.Name;
            Login = user.Login;
            Birthday = DateTime.Now;
            Gender = GenderType.Male;
            await RaisePropertyChanged(nameof(BirthdayText));
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

        private void OnSelectGender(GenderType genderType)
        {
            Gender = genderType;
        }

        private async Task OnProfileUpdateAsync()
        {
            try
            {
                IsBusy = true;

                var dataModel = new UserUpdateProfileDataModel()
                {
                    Email = this.Email,
                    Login = this.Login,
                    Name = this.Name,
                    Sex = Gender?.ToString(),
                    Birthday = Birthday?.ToShortDateString()
                };

                await _apiService.UpdateProfileAsync(dataModel);

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnChangePasswordAsync()
        {
            await _dialogService.ShowAlertAsync("Change password");
        }
    }
}
