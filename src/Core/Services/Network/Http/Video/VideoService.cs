using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading;
using Serilog;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Services.Network.Http.Video
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly IPlatformHttpClient _platformHttpClient;

        public VideoService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxMessenger messenger,
            IPlatformHttpClient platformHttpClient)
        {
            _platformHttpClient = platformHttpClient;

            var environment = environmentConfigurationProvider.Environment;
            _logger = this.Logger();

            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                _logger,
                messenger);
        }

        public async Task<VideoDto> SendVideoAsync(
            int orderId,
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

        public async Task<VideoDto> SendVideoAsync2(
            int orderId,
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

            var responseJson = await _platformHttpClient.UploadVideoAsync(loadVideoApiModel, onChangedProgressAction);
            return JsonConvert.DeserializeObject<VideoDto>(responseJson);
        }

        public async Task<long?> IncrementVideoViewsAsync(int videoId)
        {
            var videoApiModel = await _client.UnauthorizedGetAsync<ResponseDto<VideoDto>>($"videos/{videoId}/looked");
            _logger.LogDebug($"Registered {videoApiModel?.Data?.ViewsCount} for video with id {videoId}");
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