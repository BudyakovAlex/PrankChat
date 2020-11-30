using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video
{
    public interface IVideoService
    {
        Task<VideoApiModel> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default);

        Task<long?> RegisterVideoViewedFactAsync(int videoId);

        Task ComplainVideoAsync(int videoId, string title, string description);

        Task<CommentApiModel> CommentVideoAsync(int videoId, string comment);

        Task<BaseBundleApiModel<CommentApiModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize);
    }
}