using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Permissions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Core.ApplicationServices.Mediaes
{
    public class MediaService : IMediaService
    {
        private readonly IPermissionService _permissionService;
        private readonly IDialogService _dialogService;

        private bool _isCrossMediaInitialized;

        public MediaService(IPermissionService permissionService, IDialogService dialogService)
        {
            _permissionService = permissionService;
            _dialogService = dialogService;
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            var result = await _permissionService.RequestPermissionAsync<StoragePermission>();
            if (!result)
            {
                _dialogService.ShowAlertAsync("Разрешите приложению использовать хранилище.").FireAndForget();
                return null;
            }

            await Initialize();
            return await CrossMedia.Current.PickPhotoAsync();
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            var result = await _permissionService.RequestPermissionAsync<CameraPermission>();
            if (!result)
            {
                _dialogService.ShowAlertAsync("Разрешите приложению использовать камеру.").FireAndForget();
                return null;
            }

            await Initialize();
            var option = new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                AllowCropping = true,
                SaveToAlbum = true,
            };
            return await CrossMedia.Current.TakePhotoAsync(option);
        }

        public async Task<MediaFile> PickVideoAsync()
        {
            var result = await _permissionService.RequestPermissionAsync<StoragePermission>();
            if (!result)
            {
                _dialogService.ShowAlertAsync("Разрешите приложению использовать хранилище.").FireAndForget();
                return null;
            }

            await Initialize();
            return await CrossMedia.Current.PickVideoAsync();
        }

        private async Task Initialize()
        {
            if (_isCrossMediaInitialized)
                return;

            _isCrossMediaInitialized = await CrossMedia.Current.Initialize();
        }
    }
}
