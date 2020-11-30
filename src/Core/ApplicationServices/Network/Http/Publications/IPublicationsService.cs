using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications
{
    public interface IPublicationsService
    {
        Task<VideoApiModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<VideoApiModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<BaseBundleApiModel<VideoApiModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<BaseBundleApiModel<VideoApiModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<BaseBundleApiModel<VideoApiModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null);
    }
}