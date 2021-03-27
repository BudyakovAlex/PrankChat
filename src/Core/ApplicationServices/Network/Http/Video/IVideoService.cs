using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video
{
    public interface IVideoService
    {
        Task<VideoDto> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default);

        Task<long?> IncrementVideoViewsAsync(int videoId);

        Task ComplainVideoAsync(int videoId, string title, string description);

        Task<CommentDto> CommentVideoAsync(int videoId, string comment);

        Task<BaseBundleDto<CommentDto>> GetVideoCommentsAsync(int videoId, int page, int pageSize);
    }
}