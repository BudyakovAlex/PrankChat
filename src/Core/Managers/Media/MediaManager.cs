using Plugin.Media;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Permissions;
using PrankChat.Mobile.Core.Services.Dialogs;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Media
{
    //TODO: move texts to AppStrings
    public class MediaManager : IMediaManager
    {
        private readonly IPermissionProvider _permissionProvider;
        private readonly IDialogService _dialogService;

        private bool _isCrossMediaInitialized;

        public MediaService(IPermissionProvider permissionProvider, IDialogService dialogService)
        {
            _permissionProvider = permissionProvider;
            _dialogService = dialogService;
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            var result = await _permissionProvider.RequestPermissionAsync<Xamarin.Essentials.Permissions.StorageRead>();
            if (!result)
            {
                _userInteraction.ShowAlertAsync("Разрешите приложению использовать хранилище.").FireAndForget();
                return null;
            }

            await InitializeAsync();
            return await CrossMedia.Current.PickPhotoAsync();
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            var result = await _permissionProvider.RequestPermissionAsync<Xamarin.Essentials.Permissions.Camera>();
            if (!result)
            {
                _userInteraction.ShowAlertAsync("Разрешите приложению использовать камеру.").FireAndForget();
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
                var result = await _permissionProvider.RequestPermissionAsync<Xamarin.Essentials.Permissions.StorageRead>();
                if (!result)
                {
                    _userInteraction.ShowAlertAsync("Разрешите приложению использовать хранилище.").FireAndForget();
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
