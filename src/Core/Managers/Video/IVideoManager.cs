﻿using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public interface IVideoManager
    {
        Task<Models.Data.Video> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default);

        Task<long?> IncrementVideoViewsAsync(int videoId);

        Task ComplainVideoAsync(int videoId, string title, string description);

        Task<Comment> CommentVideoAsync(int videoId, string comment);

        Task<Pagination<Comment>> GetVideoCommentsAsync(int videoId, int page, int pageSize);
    }
}