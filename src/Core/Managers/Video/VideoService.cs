using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public class VideoManager : BaseRestService, IVideoManager
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public VideoManager(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<VideoManager>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default)
        {
            var loadVideoApiModel = new LoadVideoApiModel()
            {
                OrderId = orderId,
                FilePath = path,
                Title = title,
                Description = description,
            };

            var videoMetadataApiModel = await _client.PostVideoFileAsync<LoadVideoApiModel, DataApiModel<VideoApiModel>>("videos", loadVideoApiModel, onChangedProgressAction: onChangedProgressAction, cancellationToken: cancellationToken);
            return MappingConfig.Mapper.Map<VideoDataModel>(videoMetadataApiModel?.Data);
        }

        public async Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            var videoApiModel = await _client.UnauthorizedGetAsync<DataApiModel<VideoApiModel>>($"videos/{videoId}/looked");
            _log.Log(MvxLogLevel.Debug, () => $"Registered {videoApiModel?.Data?.ViewsCount} for video with id {videoId}");
            return videoApiModel?.Data?.ViewsCount;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"videos/{videoId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<CommentDataModel> CommentVideoAsync(int videoId, string comment)
        {
            var dataApiModel = new SendCommentApiModel
            {
                Text = comment,
            };

            var url = $"videos/{videoId}/comments";
            var dataModel = await _client.PostAsync<SendCommentApiModel, DataApiModel<CommentApiModel>>(url, dataApiModel);
            return MappingConfig.Mapper.Map<CommentDataModel>(dataModel?.Data);
        }

        public async Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            var data = await _client.GetAsync<BaseBundleApiModel<CommentApiModel>>($"videos/{videoId}/comments?page={page}&items_per_page={pageSize}");
            return CreatePaginationResult<CommentApiModel, CommentDataModel>(data);
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