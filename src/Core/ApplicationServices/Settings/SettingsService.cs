using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string AccessTokenKey = "access_token";

        private string _token;

        public async Task<string> GetAccessTokenAsync()
        {
            return _token;
            //return SecureStorage.GetAsync(AccessTokenKey);
        }

        public async Task SetAccessTokenAsync(string accessToken)
        {
            _token = accessToken;

            //if (string.IsNullOrWhiteSpace(accessToken))
            //{
            //    return Task.CompletedTask;
            //}

            //return SecureStorage.SetAsync(AccessTokenKey, accessToken);
        }
    }
}
