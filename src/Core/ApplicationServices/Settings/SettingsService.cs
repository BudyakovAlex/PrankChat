using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string AccessTokenKey = "access_token";

        public UserDataModel User
        {
            get => JsonConvert.DeserializeObject<UserDataModel>(Preferences.Get(Constants.Keys.User, string.Empty));
            set => Preferences.Set(nameof(User), JsonConvert.SerializeObject(value));
        }

        public string PushToken
        {
            get => Preferences.Get(nameof(PushToken), string.Empty);
            set
            {
                Preferences.Set(nameof(PushToken), value);
                IsPushTokenSend = false;
            }
        }

        public bool IsPushTokenSend
        {
            get => Preferences.Get(nameof(IsPushTokenSend), false);
            set => Preferences.Set(nameof(IsPushTokenSend), value);
        }

        public bool IsDebugMode => CheckIsDebugMode();

        private bool CheckIsDebugMode()
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif
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
