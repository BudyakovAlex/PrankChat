﻿using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video;
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

        public async Task<VideoDataModel> SendVideoAsync(
            int orderId,
            string path,
            string title,
            string description,
            Action<double, double> onChangedProgressAction = null,
            CancellationToken cancellationToken = default)
        {
            return await _videoService.SendVideoAsync(orderId, path, title, description, onChangedProgressAction, cancellationToken);
        }

        public async Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            return await _videoService.RegisterVideoViewedFactAsync(videoId);
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            return _videoService.ComplainVideoAsync(videoId, title, description);
        }

        public async Task<CommentDataModel> CommentVideoAsync(int videoId, string comment)
        {
            return await CommentVideoAsync(videoId, comment);
        }

        public async Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            return await _videoService.GetVideoCommentsAsync(videoId, page, pageSize);
        }
    }
}