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
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
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
                var user = await ApiService.SendAvatarAsync(file.Path);
                if (user == null)
                {
                    ErrorHandleService.HandleException(new BaseUserVisibleException("Ошибка при загрузке фотографии."));
                    return;
                }

                ProfilePhotoUrl = file.Path;
                SettingsService.User = user;
                _isUserPhotoUpdated = true;
                _messenger.Publish(new UpdateAvatarMessage(this));
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Логин не может быть пустым."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (Birthday == null)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("День рождения не может быть пустым."));
                return false;
            }

            if (Birthday > DateTime.Now)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Дата дня рождения не может быть польше текущей даты."));
                return false;
            }

            if ((DateTime.Now.Year - Birthday?.Year) <= 18)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Пользователь не может быть младше 18 лет."));
                return false;
            }

            if (Gender == null)
            {
                ErrorHandleService.HandleException(new BaseUserVisibleException("Выберите свой пол."));
                return false;
            }

            return true;
        }
    }
}
