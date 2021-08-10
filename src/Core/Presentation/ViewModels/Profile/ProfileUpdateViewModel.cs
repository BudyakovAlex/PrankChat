﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common.Constants;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Results;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.Core.Services.Media;
using PrankChat.Mobile.Core.Services.Notifications;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ProfileUpdateViewModel : BaseProfileViewModel, IMvxViewModelResult<ProfileUpdateResult>
    {
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IMediaService _mediaService;
        private readonly IExternalAuthService _externalAuthService;
        private readonly IPushNotificationProvider _pushNotificationService;

        private bool _isUserPhotoUpdated;

        public ProfileUpdateViewModel(
            IAuthorizationManager authorizationManager,
            IUsersManager usersManager,
            IExternalAuthService externalAuthService,
            IPushNotificationProvider pushNotificationService,
            IMediaService mediaService) : base(usersManager)
        {
            _authorizationManager = authorizationManager;
            _externalAuthService = externalAuthService;
            _pushNotificationService = pushNotificationService;
            _mediaService = mediaService;

            SaveProfileCommand = this.CreateCommand(SaveProfileAsync);
            ChangePasswordCommand = this.CreateCommand(ChangePasswordAsync);
            ShowMenuCommand = this.CreateCommand(ShowMenuAsync);
            ChangeProfilePhotoCommand = this.CreateCommand(ChangeProfilePhotoAsync);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public IMvxAsyncCommand SaveProfileCommand { get; }
        public IMvxAsyncCommand ChangePasswordCommand { get; }
        public IMvxAsyncCommand ShowMenuCommand { get; }
        public IMvxAsyncCommand ChangeProfilePhotoCommand { get; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.TrySetResult(new ProfileUpdateResult(false, _isUserPhotoUpdated));
            }

            base.ViewDestroy(viewFinishing);
        }

        private async Task SaveProfileAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            var userUpdateProfile = new UserUpdateProfile(
                Name,
                Email,
                Login,
                Gender.Value,
                Birthday?.ToShortDateString(),
                Description);

            if (_isUserPhotoUpdated)
            {
                var user = await UsersManager.SendAvatarAsync(ProfilePhotoUrl);
                if (user == null)
                {
                    ErrorHandleService.LogError(this, "User is null after avatar loading. Aborting.");
                }
            }

            UserSessionProvider.User = await UsersManager.UpdateProfileAsync(userUpdateProfile);
            await NavigationManager.CloseAsync(this, new ProfileUpdateResult(true, _isUserPhotoUpdated));
        }

        private async Task ShowMenuAsync()
        {
            // TODO: These features will be implemented.
            //var items = new string[]
            //{
            //Resources.ProfileView_Menu_Favourites,
            //Resources.ProfileView_Menu_TaskSubscriptions,
            //Resources.ProfileView_Menu_Faq,
            //Resources.ProfileView_Menu_Support,
            //Resources.ProfileView_Menu_Settings,
            //Resources.ProfileView_Menu_LogOut,
            //};

            //var result = await DialogService.ShowMenuDialogAsync(items, Resources.Cancel);
            //if (string.IsNullOrWhiteSpace(result))
            //    return;

            //if (result == Resources.ProfileView_Menu_Favourites)
            //{
            //}
            //else if (result == Resources.ProfileView_Menu_TaskSubscriptions)
            //{
            //}
            //else if (result == Resources.ProfileView_Menu_Faq)
            //{
            //}
            //else if (result == Resources.ProfileView_Menu_Support)
            //{
            //}
            //else if (result == Resources.ProfileView_Menu_Settings)
            //{
            //}
            //else if (result == Resources.ProfileView_Menu_LogOut)
            //{
            //    await LogoutUserAsync();
            //}

            var canLogout = await DialogService.ShowConfirmAsync($"{Resources.ProfileView_Menu_LogOut}?");
            if (canLogout)
            {
                await LogoutUserAsync();
            }
        }

        private async Task LogoutUserAsync()
        {
            //TODO: remove suspend notifications if unregister will be fixed on BE
            if (Xamarin.Essentials.Connectivity.NetworkAccess.HasConnection())
            {
                try
                {
                    ErrorHandleService.SuspendServerErrorsHandling();
                    await _pushNotificationService.UnregisterNotificationsAsync();
                    await _authorizationManager.LogoutAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                finally
                {
                    ErrorHandleService.ResumeServerErrorsHandling();
                }
            }

            UserSessionProvider.User = null;
            UserSessionProvider.IsPushTokenSend = false;

            await UserSessionProvider.SetAccessTokenAsync(string.Empty);

            if (Xamarin.Essentials.Connectivity.NetworkAccess.HasConnection())
            {
                _externalAuthService.LogoutFromFacebook();
                _externalAuthService.LogoutFromVkontakte();
            }

            await NavigationManager.NavigateAsync<LoginViewModel>();
        }

        private Task ChangePasswordAsync()
        {
            return NavigationManager.NavigateAsync<PasswordRecoveryViewModel>();
        }

        private async Task ChangeProfilePhotoAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.TakePhoto,
                Resources.PickPhoto,
            });

            var file = result == Resources.TakePhoto
                ? await _mediaService.TakePhotoAsync()
                : await _mediaService.PickPhotoAsync();

            if (file is null)
            {
                return;
            }

            var parameter = new ImagePathNavigationParameter(file.Path);
            var croppedImagePath = await NavigationManager.NavigateAsync<ImageCropViewModel, ImagePathNavigationParameter, ImageCropPathResult>(parameter);
            if (croppedImagePath == null)
            {
                return;
            }

            ProfilePhotoUrl = null;
            ProfilePhotoUrl = croppedImagePath.FilePath;
            _isUserPhotoUpdated = true;
            Messenger.Publish(new UpdateAvatarMessage(this));
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