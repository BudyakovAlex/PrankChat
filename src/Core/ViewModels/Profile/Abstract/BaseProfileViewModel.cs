using MvvmCross.Commands;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Common;
using System;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;

namespace PrankChat.Mobile.Core.ViewModels.Profile.Abstract
{
    public abstract class BaseProfileViewModel : PaginationViewModel
    {
        private readonly Timer _timer;

        public BaseProfileViewModel(IUsersManager usersManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            UsersManager = usersManager;

            var timeStamp = Preferences.Get(nameof(CanResendEmailValidation), DateTime.MinValue);
            _canResendEmailValidation = timeStamp <= DateTime.Now;

            _timer = new Timer(Constants.Profile.CheckCanSendEmailInterval);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();

            ResendEmailValidationCommand = this.CreateCommand(ResendEmailValidationAsync, () => CanResendEmailValidation);
            ShowValidationWarningCommand = this.CreateCommand(ShowValidationWarning);
            SelectBirthdayCommand = this.CreateCommand(SelectBirthdayAsync);
            SelectGenderCommand = this.CreateCommand<GenderType>(SelectGender);
        }

        protected IUsersManager UsersManager { get; }

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
            set => SetProperty(ref _login, value, () => RaisePropertyChanged(nameof(ProfileShortName)));
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ProfileShortName => Login.ToShortenName();

        private DateTime? _birthdate;
        public DateTime? Birthday
        {
            get => _birthdate;
            set => SetProperty(ref _birthdate, value, () => RaisePropertyChanged(nameof(BirthdayText)));
        }

        public string BirthdayText => Birthday?.ToShortDateString() ?? Resources.ProfileUpdateView_Birthday_Placeholder;

        private GenderType? _gender;
        public GenderType? Gender
        {
            get => _gender;
            set
            {
                SetProperty(ref _gender, value);
                RaisePropertiesChanged(nameof(IsGenderMale), nameof(IsGenderFemale));
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
            set => SetProperty(ref _description, value, () => RaisePropertiesChanged(nameof(HasDescription), nameof(LimitTextPresentation)));
        }

        public string LimitTextPresentation => $"{Math.Min(Constants.Profile.DescriptionMaxLength, Description?.Length ?? 0)} / {Constants.Profile.DescriptionMaxLength}";

        public bool HasDescription => !string.IsNullOrEmpty(Description);

        public IMvxAsyncCommand ResendEmailValidationCommand { get; }

        public IMvxCommand ShowValidationWarningCommand { get; }

        public IMvxAsyncCommand SelectBirthdayCommand { get; }

        public IMvxCommand<GenderType> SelectGenderCommand { get; }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(InitializeProfileData(), base.InitializeAsync());
        }

        protected virtual Task InitializeProfileData()
        {
            var user = UserSessionProvider.User;
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
            await UsersManager.VerifyEmailAsync();

            UserInteraction.ShowToast(Resources.Profile_Email_Confirmation_Sent, ToastType.Positive);
            Preferences.Set(nameof(CanResendEmailValidation), DateTime.Now.AddMinutes(Constants.Profile.UnlockResendMinutes));
            CanResendEmailValidation = false;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var timeStamp = Preferences.Get(nameof(CanResendEmailValidation), DateTime.MinValue);
            CanResendEmailValidation = timeStamp <= DateTime.Now;
        }

        private void ShowValidationWarning()
        {
            UserInteraction.ShowToast(Resources.Profile_Your_Email_Not_Actual, ToastType.Negative);
        }

        private async Task SelectBirthdayAsync()
        {
            var result = await UserInteraction.ShowDateDialogAsync(Birthday);
            if (result is null)
            {
                return;
            }

            Birthday = result.Value;
        }

        private void SelectGender(GenderType genderType)
        {
            Gender = genderType;
        }
    }
}