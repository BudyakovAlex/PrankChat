using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications
{
    public interface IPublicationsService
    {
        Task<VideoDto> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<VideoDto> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<BaseBundleDto<VideoDto>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<BaseBundleDto<VideoDto>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<BaseBundleDto<VideoDto>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null);
    }
}