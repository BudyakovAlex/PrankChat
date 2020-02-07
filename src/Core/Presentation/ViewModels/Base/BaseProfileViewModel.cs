using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public class BaseProfileViewModel : BaseViewModel
    {
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
            set
            {
                SetProperty(ref _gender, value);
                RaisePropertyChanged(nameof(IsGenderMale));
                RaisePropertyChanged(nameof(IsGenderFemale));
            }
        }

        public bool IsGenderMale => Gender == GenderType.Male;

        public bool IsGenderFemale => Gender == GenderType.Female;

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

        protected ISettingsService SettingsService { get; set; }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<GenderType> SelectGenderCommand => new MvxCommand<GenderType>(OnSelectGender);


        public BaseProfileViewModel(INavigationService navigationService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            SettingsService = settingsService;
        }

        public override Task Initialize()
        {
            InitializeProfileData();
            return base.Initialize();
        }

        private async Task OnSelectBirthdayAsync()
        {
            var result = await DialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                Birthday = result.Value;
            }
        }

        private void OnSelectGender(GenderType genderType)
        {
            Gender = genderType;
        }

        protected virtual void InitializeProfileData()
        {
            var user = SettingsService.User;
            if (user == null)
                return;

            Email = user.Email;
            Name = user.Name;
            Login = user.Login;
            Birthday = user.Birthday;
            Gender = user.Sex;
            ProfilePhotoUrl = user.Avatar;
            Description = user.Description;
        }
    }
}
