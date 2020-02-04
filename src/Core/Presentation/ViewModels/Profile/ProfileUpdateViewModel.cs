using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
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
        private readonly IMvxMessenger _messenger;
        private readonly IMediaService _mediaService;

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
                                      IMvxMessenger messenger,
                                      IMediaService mediaService,
                                      IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _mediaService = mediaService;
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

        private async Task OnUpdateProfileAsync()
        {
            if (!CheckValidation())
                return;

            try
            {
                IsBusy = true;

                var dataModel = new UserUpdateProfileDataModel()
                {
                    Email = Email,
                    Login = Login,
                    Name = Name,
                    Sex = Gender.Value,
                    Birthday = Birthday?.ToShortDateString(),
                    Description = Description
                };

                _settingsService.User = await ApiService.UpdateProfileAsync(dataModel);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnChangePasswordAsync()
        {
            await DialogService.ShowAlertAsync("Change password");
        }

        private async Task OnChangeProfilePhotoAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.TakePhoto,
                Resources.PickPhoto,
            });

            MediaFile file = null;
            if (result == Resources.TakePhoto)
            {
                file = await _mediaService.TakePhotoAsync();
            }
            else if (result == Resources.PickPhoto)
            {
                file = await _mediaService.PickPhotoAsync();
            }

            if (file != null)
            {
                var user = await ApiService.SendAvatarAsync(file.Path);
                if (user == null)
                {
                    ErrorHandleService.HandleException(new UserVisibleException("Ошибка при загрузке фотографии."));
                    return;
                }

                ProfilePhotoUrl = file.Path;
                _settingsService.User = user;
                _messenger.Publish(new UpdateAvatarMessage(this));
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorHandleService.HandleException(new UserVisibleException("Логин не может быть пустым."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new UserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new UserVisibleException("День рождения не может быть пустым."));
                return false;
            }

            if (Birthday > DateTime.Now)
            {
                ErrorHandleService.HandleException(new UserVisibleException("Дата дня рождения не может быть польше текущей даты."));
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= 18)
            {
                ErrorHandleService.HandleException(new UserVisibleException("Пользователь не может быть младше 18 лет."));
                return false;
            }

            if (Gender == null)
            {
                ErrorHandleService.HandleException(new UserVisibleException("Выберите свой пол."));
                return false;
            }

            return true;
        }
    }
}
