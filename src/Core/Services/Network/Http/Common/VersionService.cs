using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using Serilog;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Services.Network.Http.Common
{
    public class VersionService : IVersionService
    {
        private readonly HttpClient _client;

        public VersionService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxMessenger messenger) 
        {
            var environment = environmentConfigurationProvider.Environment;
            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                this.Logger(),
                messenger);
        }

        public async Task<AppVersionDto> CheckAppVersionAsync()
        {
            var buildVersion = AppInfo.BuildString;
            var appVersion = AppInfo.VersionString;
            var operationSystem = DeviceInfo.Platform.ToString().ToLower();
            var appVersionBundle = await _client.UnauthorizedGetAsync<AppVersionDto>($"/application/{buildVersion}/check/{operationSystem}?appVersion={appVersion}");
            if (appVersionBundle is null)
            {
                return new AppVersionDto();
            }

            return appVersionBundle;
        }
    }
}