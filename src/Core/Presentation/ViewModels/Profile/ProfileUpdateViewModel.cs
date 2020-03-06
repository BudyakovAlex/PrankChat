using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileUpdateViewModel : BaseProfileViewModel, IMvxViewModelResult<ProfileUpdateResult>
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMediaService _mediaService;

        private bool _isUserPhotoUpdated;

        public bool IsProfilePhotoUploading { get; set; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public MvxAsyncCommand SaveProfileCommand => new MvxAsyncCommand(OnSaveProfileAsync);

        public MvxAsyncCommand ChangePasswordCommand => new MvxAsyncCommand(OnChangePasswordAsync);

        public MvxAsyncCommand ChangeProfilePhotoCommand => new MvxAsyncCommand(OnChangeProfilePhotoAsync);

        public ProfileUpdateViewModel(INavigationService navigationService,
                                      ISettingsService settingsService,
                                      IDialogService dialogService,
                                      IApiService apiService,
                                      IMvxMessenger messenger,
                                      IMediaService mediaService,
                                      IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _messenger = messenger;
            _mediaService = mediaService;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetResult(new ProfileUpdateResult(false, _isUserPhotoUpdated));

            base.ViewDestroy(viewFinishing);
        }

        private async Task OnSaveProfileAsync()
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

                if (_isUserPhotoUpdated)
                {
                    var user = await ApiService.SendAvatarAsync(ProfilePhotoUrl);
                    if (user == null)
                    {
                        ErrorHandleService.LogError(this, "User is null after avatar loading. Aborting.");
                    }
                }

                SettingsService.User = await ApiService.UpdateProfileAsync(dataModel);

                CloseCompletionSource.SetResult(new ProfileUpdateResult(true, _isUserPhotoUpdated));
                await NavigationService.CloseView(this);
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
                var croppedImagePath = await NavigationService.ShowImageCropView(file.Path);
                if (croppedImagePath == null)
                    return;

                ProfilePhotoUrl = null;
                ProfilePhotoUrl = croppedImagePath.FilePath;
                _isUserPhotoUpdated = true;
                _messenger.Publish(new UpdateAvatarMessage(this));
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Login, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Login can't be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Name, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Name can't be empty.");
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Birthday, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Birthday can't be empty.");
                return false;
            }

            if (Birthday > DateTime.Now)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Birthday, ValidationErrorType.GreaterThanRequired));
                ErrorHandleService.LogError(this, "Birthday date can't be greater than current date.");
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= Constants.Age.AdultAge)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Birthday, ValidationErrorType.LowerThanRequired, Constants.Age.AdultAge.ToString()));
                ErrorHandleService.LogError(this, $"User can't be younger than {Constants.Age.AdultAge} years.");
                return false;
            }

            if (Gender == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Gender, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Gender can't be empty.");
                return false;
            }

            return true;
        }
    }
}
