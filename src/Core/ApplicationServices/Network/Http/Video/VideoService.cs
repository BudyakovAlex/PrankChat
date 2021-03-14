using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video
{
    public class VideoService : BaseRestService, IVideoService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public VideoService(
            IUserSessionProvider userSessionProvider,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger) : base(userSessionProvider, authorizeService, logProvider, messenger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<VideoService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(
                configuration.BaseAddress,
                configuration.ApiVersion,
                userSessionProvider,
                _log,
                messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
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

            var videoMetadataApiModel = await _client.PostVideoFileAsync<UploadVideoDto, ResponseDto<VideoDto>>("videos",
                                                                                                                         loadVideoApiModel,
                                                                                                                         onChangedProgressAction: onChangedProgressAction,
                                                                                                                         cancellationToken: cancellationToken);
            return videoMetadataApiModel?.Data;
        }

        public async Task<long?> RegisterVideoViewedFactAsync(int videoId)
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