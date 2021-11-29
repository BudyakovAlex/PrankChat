using Plugin.Media;
using Plugin.Media.Abstractions;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Core.Providers.Permissions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Managers.Media
{
    //TODO: move texts to AppStrings
    public class MediaManager : IMediaManager
    {
        private readonly IPermissionProvider _permissionProvider;
        private readonly IUserInteraction _userInteraction;

        private bool _isCrossMediaInitialized;

        public MediaManager(IPermissionProvider permissionProvider, IUserInteraction userInteraction)
        {
            _permissionProvider = permissionProvider;
            _userInteraction = userInteraction;
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
            return await MainThread.InvokeOnMainThreadAsync(() => CrossMedia.Current.PickPhotoAsync());
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
            return await MainThread.InvokeOnMainThreadAsync(() => CrossMedia.Current.TakePhotoAsync(option));
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
                return await MainThread.InvokeOnMainThreadAsync(() => CrossMedia.Current.PickVideoAsync());
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        private Task InitializeAsync()
        {
            if (_isCrossMediaInitialized)
            {
                return Task.CompletedTask;
            }

            return MainThread.InvokeOnMainThreadAsync(async () =>
            {
                _isCrossMediaInitialized = await CrossMedia.Current.Initialize();
            });
        }
    }
}
