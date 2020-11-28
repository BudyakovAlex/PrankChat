using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video;
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

        public Task<VideoDataModel> SendVideoAsync(int orderId,
                                                   string path,
                                                   string title,
                                                   string description,
                                                   Action<double, double> onChangedProgressAction = null,
                                                   CancellationToken cancellationToken = default)
        {
            return _videoService.SendVideoAsync(orderId, path, title, description, onChangedProgressAction, cancellationToken);
        }

        public Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            return _videoService.RegisterVideoViewedFactAsync(videoId);
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            return _videoService.ComplainVideoAsync(videoId, title, description);
        }

        public Task<CommentDataModel> CommentVideoAsync(int videoId, string comment)
        {
            return _videoService.CommentVideoAsync(videoId, comment);
        }

        public Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            return _videoService.GetVideoCommentsAsync(videoId, page, pageSize);
        }
    }
}