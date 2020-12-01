using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public class VideoManager : IVideoManager
    {
        private readonly IVideoService _videoService;

        public VideoManager(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<VideoDataModel> SendVideoAsync(int orderId,
                                                   string path,
                                                   string title,
                                                   string description,
                                                   Action<double, double> onChangedProgressAction = null,
                                                   CancellationToken cancellationToken = default)
        {
            var response = await _videoService.SendVideoAsync(orderId, path, title, description, onChangedProgressAction, cancellationToken);
            return response.Map();
        }

        public Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            return _videoService.RegisterVideoViewedFactAsync(videoId);
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            return _videoService.ComplainVideoAsync(videoId, title, description);
        }

        public async Task<CommentDataModel> CommentVideoAsync(int videoId, string comment)
        {
            var response = await _videoService.CommentVideoAsync(videoId, comment);
            return response.Map();
        }

        public async Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            var response = await _videoService.GetVideoCommentsAsync(videoId, page, pageSize);
            return response.Map(item => item.Map());
        }
    }
}