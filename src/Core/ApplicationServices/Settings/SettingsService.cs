using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.Models.Data;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string AccessTokenKey = "access_token";

        public UserDataModel User
        {
            get => JsonConvert.DeserializeObject<UserDataModel>(Preferences.Get(nameof(User), string.Empty));
            set => Preferences.Set(nameof(User), JsonConvert.SerializeObject(value));
        }

        public Task<string> GetAccessTokenAsync()
        {
            // Workaround for iOS simulator.
            if (CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS
                && !CrossDeviceInfo.Current.IsDevice)
            {
                return Task.FromResult(Preferences.Get(AccessTokenKey, string.Empty));
            }

            return SecureStorage.GetAsync(AccessTokenKey);
        }

        public Task SetAccessTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            // Workaround for iOS simulator.
            if (CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS
                && !CrossDeviceInfo.Current.IsDevice)
            {
                Preferences.Set(AccessTokenKey, accessToken);
                return Task.CompletedTask;
            }

            // Workaround for iOS simulator.
            if (CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS
                && !CrossDeviceInfo.Current.IsDevice)
            {
                Preferences.Set(AccessTokenKey, accessToken);
                return Task.CompletedTask;
            }

            return SecureStorage.SetAsync(AccessTokenKey, accessToken);
        }
    }
}
