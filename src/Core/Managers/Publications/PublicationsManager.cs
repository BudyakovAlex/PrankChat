using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Managers.Publications
{
    public class PublicationsManager : IPublicationsManager
    {
        private readonly IPublicationsService _publicationsService;

        public PublicationsManager(IPublicationsService publicationsService)
        {
            _publicationsService = publicationsService;
        }

        public Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            return _publicationsService.GetPopularVideoFeedAsync(dateFilterType, page, pageSize);
        }

        public Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            return _publicationsService.GetActualVideoFeedAsync(dateFilterType, page, pageSize);
        }

        public Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            return _publicationsService.GetMyVideoFeedAsync(page, pageSize, dateFilterType);
        }

        public Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            return _publicationsService.SendLikeAsync(videoId, isChecked, cancellationToken);
        }

        public Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            return _publicationsService.SendDislikeAsync(videoId, isChecked, cancellationToken);
        }
    }
}