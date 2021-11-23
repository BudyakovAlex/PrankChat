using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Search
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _client;

        public SearchService(
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