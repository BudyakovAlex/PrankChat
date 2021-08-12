using Plugin.Media;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Services.Dialogs;
using PrankChat.Mobile.Core.Services.Permissions;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Media
{
    //TODO: move texts to AppStrings
    public class MediaManager : IMediaManager
    {
        private readonly IPermissionService _permissionService;
        private readonly IDialogService _dialogService;

        private bool _isCrossMediaInitialized;

        public MediaManager(IPermissionService permissionService, IDialogService dialogService)
        {
            _permissionService = permissionService;
            _dialogService = dialogService;
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            var result = await _permissionService.RequestPermissionAsync<Xamarin.Essentials.Permissions.StorageRead>();
            if (!result)
            {
                _dialogService.ShowAlertAsync(Resources.Allow_Storage).FireAndForget();
                return null;
            }

            await InitializeAsync();
            return await CrossMedia.Current.PickPhotoAsync();
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            var result = await _permissionService.RequestPermissionAsync<Xamarin.Essentials.Permissions.Camera>();
            if (!result)
            {
                _dialogService.ShowAlertAsync(Resources.Allow_Camera).FireAndForget();
                return null;
            }

            await InitializeAsync();
            var option = new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                SaveToAlbum = true,
                SaveMetaData = false,
            };
            return await CrossMedia.Current.TakePhotoAsync(option);
        }

        public async Task<MediaFile> PickVideoAsync()
        {
            try
            {
                var result = await _permissionService.RequestPermissionAsync<Xamarin.Essentials.Permissions.StorageRead>();
                if (!result)
                {
                    _dialogService.ShowAlertAsync(Resources.Allow_Storage).FireAndForget();
                    return null;
                }

                await InitializeAsync();
                return await CrossMedia.Current.PickVideoAsync();
            }
            catch
            {
                return null;
            }
        }

        private async Task InitializeAsync()
        {
            if (_isCrossMediaInitialized)
            {
                return;
            }

            _isCrossMediaInitialized = await CrossMedia.Current.Initialize();
        }
    }
}
