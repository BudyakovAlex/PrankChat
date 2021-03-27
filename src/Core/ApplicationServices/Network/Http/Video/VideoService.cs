using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _client;
        private readonly IMvxLog _log;

        public VideoService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            _log = logProvider.GetLogFor<VideoService>();

            var environment = environmentConfigurationProvider.Environment;

            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                _log,
                messenger);
        }

        public async Task<VideoDto> SendVideoAsync(int orderId,
                                                        string path,
                                                        string title,
                                                        string description,
                                                        Action<double, double> onChangedProgressAction = null,
                                                        CancellationToken cancellationToken = default)
        {
            var loadVideoApiModel = new UploadVideoDto()
            {
                OrderId = orderId,
                FilePath = path,
                Title = title,
                Description = description,
            };

            var videoMetadataApiModel = await _client.PostVideoFileAsync<UploadVideoDto, ResponseDto<VideoDto>>(
                "videos",
                loadVideoApiModel,
                onChangedProgressAction: onChangedProgressAction,
                cancellationToken: cancellationToken);
            return videoMetadataApiModel?.Data;
        }

        public async Task<long?> IncrementVideoViewsAsync(int videoId)
        {
            var videoApiModel = await _client.UnauthorizedGetAsync<ResponseDto<VideoDto>>($"videos/{videoId}/looked");
            _log.Log(MvxLogLevel.Debug, () => $"Registered {videoApiModel?.Data?.ViewsCount} for video with id {videoId}");
            return videoApiModel?.Data?.ViewsCount;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            var dataApiModel = new ComplainDto()
            {
                Title = title,
                Description = description
            };
            var url = $"videos/{videoId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<CommentDto> CommentVideoAsync(int videoId, string comment)
        {
            var dataApiModel = new SendCommentDto
            {
                Text = comment,
            };

            var url = $"videos/{videoId}/comments";
            var response = await _client.PostAsync<SendCommentDto, ResponseDto<CommentDto>>(url, dataApiModel);
            return response?.Data;
        }

        public Task<BaseBundleDto<CommentDto>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            return _client.GetAsync<BaseBundleDto<CommentDto>>($"videos/{videoId}/comments?page={page}&items_per_page={pageSize}");
        }
    }
}