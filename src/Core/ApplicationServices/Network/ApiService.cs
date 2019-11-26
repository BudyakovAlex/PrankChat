using System.Threading.Tasks;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
        private readonly IMvxLog _log;
        private readonly ISettingsService _settingsService;

        public ApiService(ISettingsService settingsService, IMvxLogProvider logProvider)
        {
            _settingsService = settingsService;
            _log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress, configuration.ApiVersion, _settingsService, _log);
        }

        public Task AuthorizeAsync(string email, string password)
        {
            var loginModel = new LoginApiModel { Email = email, Password = password };
            return _client.UnauthorizedPost("auth/login", loginModel);
        }
    }
}
