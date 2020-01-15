using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string AccessTokenKey = "access_token";

        public Task<string> GetAccessTokenAsync()
        {
            return SecureStorage.GetAsync(AccessTokenKey);
        }

        public Task SetAccessTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Task.CompletedTask;
            }

            return SecureStorage.SetAsync(AccessTokenKey, accessToken);
        }
    }
}
