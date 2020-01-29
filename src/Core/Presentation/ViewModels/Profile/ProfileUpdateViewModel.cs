using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileUpdateViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IMvxMessenger _messenger;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _login;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    RaisePropertyChanged(nameof(ProfileShortName));
                }
            }
        }

        public string ProfileShortName => Name.ToShortenName();

        private DateTime? _birthdate;
        public DateTime? Birthday
        {
            get => _birthdate;
            set
            {
                if (SetProperty(ref _birthdate, value))
                {
                    RaisePropertyChanged(nameof(BirthdayText));
                }
            }
        }

        public string BirthdayText => Birthday?.ToShortDateString() ?? Resources.ProfileUpdateView_Birthday_Placeholder;

        private GenderType? _gender;
        public GenderType? Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _profilePhotoUrl;
        public string ProfilePhotoUrl
        {
            get => _profilePhotoUrl;
            set => SetProperty(ref _profilePhotoUrl, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<GenderType> SelectGenderCommand => new MvxCommand<GenderType>(OnSelectGender);

        public MvxAsyncCommand UpdateProfileCommand => new MvxAsyncCommand(OnUpdateProfileAsync);

        public MvxAsyncCommand ChangePasswordCommand => new MvxAsyncCommand(OnChangePasswordAsync);

        public MvxAsyncCommand ChangeProfilePhotoCommand => new MvxAsyncCommand(OnChangeProfilePhotoAsync);

        public ProfileUpdateViewModel(INavigationService navigationService,
                                      ISettingsService settingsService,
                                      IDialogService dialogService,
                                      IApiService apiService,
                                      IMvxMessenger messenger) : base(navigationService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _apiService = apiService;
            _messenger = messenger;
        }

        public override Task Initialize()
        {
            InitializeProfile();
            return base.Initialize();
        }

        private void InitializeProfile()
        {
            var user = _settingsService.User;

            if (user == null)
                return;

            // TODO set some properties from user
            Email = user.Email;
            Name = user.Name;
            Login = user.Login;
            Birthday = DateTime.Now;
            Gender = GenderType.Male;
            ProfilePhotoUrl = user.Avatar;
            Description = "Description";
        }

        private async Task OnSelectBirthdayAsync()
        {
            var result = await _dialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                Birthday = result.Value;
            }
        }

        private void OnSelectGender(GenderType genderType)
        {
            Gender = genderType;
        }

        private async Task OnUpdateProfileAsync()
        {
            try
            {
                IsBusy = true;

                var dataModel = new UserUpdateProfileDataModel()
                {
                    Email = Email,
                    Login = Login,
                    Name = Name,
                    Sex = Gender?.ToString(),
                    Birthday = Birthday?.ToShortDateString(),
                    Description = Description
                };

                await _apiService.UpdateProfileAsync(dataModel);

                _messenger.Publish(new UpdateUserProfileMessage(this));
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

        private async Task OnChangeProfilePhotoAsync()
        {
            await _dialogService.ShowAlertAsync("Change photo profile");
        }
    }
}
