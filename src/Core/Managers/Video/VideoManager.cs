using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Presentation.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public class VideoManager : IVideoManager
    {
        private readonly IVideoService _videoService;
        private readonly IMvxMessenger _mvxMessenger;

        public VideoManager(IVideoService videoService, IMvxMessenger mvxMessenger)
        {
            _videoService = videoService;
            _mvxMessenger = mvxMessenger;
        }

        public async Task<Models.Data.Video> SendVideoAsync(
            int orderId,
            string path,
            string title,
            string description,
            Action<double, double> onChangedProgressAction = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _videoService.SendVideoAsync(orderId, path, title, description, onChangedProgressAction, cancellationToken);
            return response.Map();
        }

        public async Task<long?> IncrementVideoViewsAsync(int videoId)
        {
            var views = await _videoService.IncrementVideoViewsAsync(videoId);
            if (views.HasValue)
            {
                _mvxMessenger.Publish(new ViewCountMessage(this, videoId, views.Value));
            }

            return views;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            return _videoService.ComplainVideoAsync(videoId, title, description);
        }

        public async Task<Comment> CommentVideoAsync(int videoId, string comment)
        {
            var response = await _videoService.CommentVideoAsync(videoId, comment);
            return response.Map();
        }

        public async Task<Pagination<Comment>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            var response = await _videoService.GetVideoCommentsAsync(videoId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Models.Data.Video> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _videoService.SendLikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }

        public async Task<Models.Data.Video> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _videoService.SendDislikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }
    }
}