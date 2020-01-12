using System.Collections.Generic;
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
            var authTokenModel = await _client.UnauthorizedPost<AuthorizationApiModel, DataApiModel<AccessTokenApiModel>>("auth/login", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            var registrationApiModel = MappingConfig.Mapper.Map<UserRegistrationApiModel>(userInfo);
            var lol = await _client.UnauthorizedPost("auth/register", registrationApiModel, true);
        }

        public Task CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            return _client.Post("orders", createOrderApiModel);
        }

        public async Task<List<OrderApiModel>> GetOrdersAsync()
        {
            var data = await _client.Get<DataApiModel<List<OrderApiModel>>>("orders");
            return data.Data;
        }
    }
}
