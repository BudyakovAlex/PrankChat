using System;
using System.Threading.Tasks;
using System.Timers;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public class BaseProfileViewModel : PaginationViewModel
    {
        private const int CheckCanSendEmailInterval = 60000;
        private const int UnlockResendMinutes = 3;

        private Timer _timer;

        public BaseProfileViewModel(INavigationService navigationService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    ISettingsService settingsService)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            ResendEmailValidationCommand = new MvxAsyncCommand(ResendEmailValidationAsync, () => CanResendEmailValidation);
            ShowValidationWarningCommand = new MvxCommand(ShowValidationWarning);

            var timeStamp = Preferences.Get(nameof(CanResendEmailValidation), DateTime.MinValue);
            _canResendEmailValidation = timeStamp <= DateTime.Now;

            _timer = new Timer(CheckCanSendEmailInterval);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

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

        private bool _isEmailVerified;
        public bool IsEmailVerified
        {
            get => _isEmailVerified;
            private set => SetProperty(ref _isEmailVerified, value);
        }

        private bool _canResendEmailValidation;
        public bool CanResendEmailValidation
        {
            get => _canResendEmailValidation;
            private set
            {
                SetProperty(ref _canResendEmailValidation, value);
                ResendEmailValidationCommand.RaiseCanExecuteChanged();
            }
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

        public IMvxAsyncCommand ResendEmailValidationCommand { get; }

        public IMvxCommand ShowValidationWarningCommand { get; }

        public MvxAsyncCommand SelectBirthdayCommand => new MvxAsyncCommand(OnSelectBirthdayAsync);

        public MvxCommand<GenderType> SelectGenderCommand => new MvxCommand<GenderType>(OnSelectGender);

        public override Task Initialize()
        {
            return Task.WhenAll(InitializeProfileData(), base.Initialize());
        }

        protected virtual Task InitializeProfileData()
        {
            var user = SettingsService.User;
            if (user == null)
            {
                return Task.CompletedTask;
            }

            IsEmailVerified = user.EmailVerifiedAt != null;
            Email = user.Email;
            Name = user.Name;
            Login = user.Login;
            Birthday = user.Birthday;
            Gender = user.Sex;
            ProfilePhotoUrl = user.Avatar;
            Description = user.Description;

            return Task.CompletedTask;
        }

        private async Task ResendEmailValidationAsync()
        {
            await ApiService.VerifyEmailAsync();

            DialogService.ShowToast(Resources.Profile_Email_Confirmation_Sent, ToastType.Positive);
            Preferences.Set(nameof(CanResendEmailValidation), DateTime.Now.AddMinutes(UnlockResendMinutes));
            CanResendEmailValidation = false;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var timeStamp = Preferences.Get(nameof(CanResendEmailValidation), DateTime.MinValue);
            CanResendEmailValidation = timeStamp <= DateTime.Now;
        }

        private void ShowValidationWarning()
        {
            DialogService.ShowToast(Resources.Profile_Your_Email_Not_Actual, ToastType.Negative);
        }

        private async Task OnSelectBirthdayAsync()
        {
            var result = await DialogService.ShowDateDialogAsync(Birthday);
            if (result.HasValue)
            {
                Birthday = result.Value;
            }
        }

        private void OnSelectGender(GenderType genderType)
        {
            Gender = genderType;
        }
    }
}