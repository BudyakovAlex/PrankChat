using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
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

        public Task<BaseBundleDto<VideoDto>> SearchVideosAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/videos?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleDto<VideoDto>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
        }

        public Task<BaseBundleDto<UserDto>> SearchUsersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/users?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleDto<UserDto>>(endpoint);
        }

        public Task<BaseBundleDto<OrderDto>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/orders?text={query}&page={page}&items_per_page={pageSize}";
            return _client.GetAsync<BaseBundleDto<OrderDto>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
        }
    }
}