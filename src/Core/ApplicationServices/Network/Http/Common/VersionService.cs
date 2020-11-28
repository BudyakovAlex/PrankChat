using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common
{
    public class VersionService : BaseRestService, IVersionService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public VersionService(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<VersionService>();

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
    }
}