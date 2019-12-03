using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly HttpClient _client;

        public ApiService(ISettingsService settingsService, IMvxLogProvider logProvider, IMvxMessenger messenger)
        {
            _settingsService = settingsService;
            var log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress, configuration.ApiVersion, settingsService, log, messenger);
        }

        public async Task AuthorizeAsync(string email, string password)
        {
            var loginModel = new AuthorizationApiModel { Email = email, Password = password };
            var authTokenModel = await _client.UnauthorizedPost<AuthorizationApiModel, AccessTokenApiModel>("auth/login", loginModel);
            await _settingsService.SetAccessTokenAsync(authTokenModel.AccessToken);
        }

        public Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            var registrationApiModel = MappingConfig.Mapper.Map<UserRegistrationApiModel>(userInfo);
            return _client.UnauthorizedPost("auth/register", registrationApiModel);
        }
    }
}
