using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
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
    public class PublicationsService : BaseRestService, IPublicationsService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public PublicationsService(ISettingsService settingsService,
                                   IAuthorizationService authorizeService,
                                   IMvxLogProvider logProvider,
                                   IMvxMessenger messenger,
                                   ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<PublicationsService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<BaseBundleApiModel<VideoApiModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/popular?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<BaseBundleApiModel<VideoApiModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/new?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<BaseBundleApiModel<VideoApiModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            if (_settingsService.User == null)
            {
                return new BaseBundleApiModel<VideoApiModel>();
            }

            var endpoint = $"newsline/my?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<VideoApiModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.PostAsync<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return data?.Data;
        }

        public async Task<VideoApiModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/dislike" : $"videos/{videoId}/dislike/remove";
            var data = await _client.PostAsync<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return data?.Data;
        }
    }
}