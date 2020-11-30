using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public interface IVideoManager
    {
        Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default);

        Task<long?> RegisterVideoViewedFactAsync(int videoId);

        Task ComplainVideoAsync(int videoId, string title, string description);

        Task<CommentDataModel> CommentVideoAsync(int videoId, string comment);

        Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize);
    }
}