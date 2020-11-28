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
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search
{
    public class SearchService : BaseRestService, ISearchService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public SearchService(
            ISettingsService settingsService,
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

        public async Task<PaginationModel<VideoDataModel>> SearchVideosAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/videos?text={query}&page={page}&items_per_page={pageSize}";
            var data = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
            return CreatePaginationResult<VideoApiModel, VideoDataModel>(data);
        }

        public async Task<PaginationModel<UserDataModel>> SearchUsersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/users?text={query}&page={page}&items_per_page={pageSize}";
            var data = await _client.GetAsync<BaseBundleApiModel<UserApiModel>>(endpoint);
            return CreatePaginationResult<UserApiModel, UserDataModel>(data);
        }

        public async Task<PaginationModel<OrderDataModel>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            var endpoint = $"search/orders?text={query}&page={page}&items_per_page={pageSize}";
            var data = await _client.GetAsync<BaseBundleApiModel<OrderApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor });
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data ?? new List<TApiModel>());
            var paginationData = data?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}