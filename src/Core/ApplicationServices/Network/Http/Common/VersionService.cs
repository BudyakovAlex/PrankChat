using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorize;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common
{
    public class VersionService : IVersionService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;
        private readonly IAuthorizeService _authorizeService;

        private readonly HttpClient _client;

        public VersionService(ISettingsService settingsService,
                          IAuthorizeService authorizeService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger,
                          ILogger logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<AuthorizeService>();
            _authorizeService = authorizeService;

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<AppVersionDataModel> CheckAppVersionAsync()
        {
            var buildVersion = AppInfo.BuildString;
            var appVersion = AppInfo.VersionString;
            var operationSystem = DeviceInfo.Platform.ToString().ToLower();
            var appVersionBundle = await _client.UnauthorizedGetAsync<AppVersionApiModel>($"/application/{buildVersion}/check/{operationSystem}?appVersion={appVersion}");
            if (appVersionBundle is null)
            {
                return new AppVersionDataModel();
            }

            return MappingConfig.Mapper.Map<AppVersionDataModel>(appVersionBundle);
        }

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
            {
                return;
            }

            _authorizeService.RefreshTokenAsync().FireAndForget();
        }
    }
}