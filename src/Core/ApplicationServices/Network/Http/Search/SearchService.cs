using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search
{
    public class SearchService : BaseRestService, ISearchService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public SearchService(ISettingsService settingsService,
                             IAuthorizationService authorizeService,
                             IMvxLogProvider logProvider,
                             IMvxMessenger messenger,
                             ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<SearchService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public Task<BaseBundleApiModel<VideoApiModel>> SearchVideosAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/videos?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
        }

        public Task<BaseBundleApiModel<UserApiModel>> SearchUsersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/users?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleApiModel<UserApiModel>>(endpoint);
        }

        public Task<BaseBundleApiModel<OrderApiModel>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/orders?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleApiModel<OrderApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
        }
    }
}