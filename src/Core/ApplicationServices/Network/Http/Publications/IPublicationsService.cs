using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications
{
    public interface IPublicationsService
    {
        Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null);
    }
}