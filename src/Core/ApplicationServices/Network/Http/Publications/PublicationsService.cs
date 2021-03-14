using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications
{
    public class PublicationsService : IPublicationsService
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly HttpClient _client;

        public PublicationsService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            _userSessionProvider = userSessionProvider;
            var log = logProvider.GetLogFor<PublicationsService>();

            var environment = environmentConfigurationProvider.Environment;

            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                log,
                messenger);
        }

        public async Task<BaseBundleDto<VideoDto>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/popular?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _userSessionProvider.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleDto<VideoDto>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleDto<VideoDto>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<BaseBundleDto<VideoDto>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/new?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _userSessionProvider.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleDto<VideoDto>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleDto<VideoDto>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<BaseBundleDto<VideoDto>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            if (_userSessionProvider.User == null)
            {
                return new BaseBundleDto<VideoDto>();
            }

            var endpoint = $"newsline/my?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = await _client.GetAsync<BaseBundleDto<VideoDto>>(endpoint, false, IncludeType.Customer);

            return videoMetadataBundle;
        }

        public async Task<VideoDto> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.PostAsync<ResponseDto<VideoDto>>(url, cancellationToken: cancellationToken);
            return data?.Data;
        }

        public async Task<VideoDto> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/dislike" : $"videos/{videoId}/dislike/remove";
            var data = await _client.PostAsync<ResponseDto<VideoDto>>(url, cancellationToken: cancellationToken);
            return data?.Data;
        }
    }
}