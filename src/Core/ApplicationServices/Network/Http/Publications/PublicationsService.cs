using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications
{
    public class PublicationsService : IPublicationsService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;
        private readonly IAuthorizationService _authorizeService;

        private readonly HttpClient _client;

        public PublicationsService(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<PublicationsService>();
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

        public async Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/popular?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            return CreatePaginationResult<VideoApiModel, VideoDataModel>(videoMetadataBundle);
        }

        public async Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/new?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            var mappedModels = MappingConfig.Mapper.Map<List<VideoDataModel>>(videoMetadataBundle?.Data);
            var paginationData = videoMetadataBundle?.Meta.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;

            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public async Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            if (_settingsService.User == null)
            {
                return new PaginationModel<VideoDataModel>();
            }

            var endpoint = $"newsline/my?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            var mappedModels = MappingConfig.Mapper.Map<List<VideoDataModel>>(videoMetadataBundle?.Data);
            var paginationData = videoMetadataBundle.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? mappedModels.Count;

            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public async Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.PostAsync<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return MappingConfig.Mapper.Map<VideoDataModel>(data?.Data);
        }

        public async Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/dislike" : $"videos/{videoId}/dislike/remove";
            var data = await _client.PostAsync<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return MappingConfig.Mapper.Map<VideoDataModel>(data?.Data);
        }

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
            {
                return;
            }

            _authorizeService.RefreshTokenAsync().FireAndForget();
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