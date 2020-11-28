using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Publications
{
    public class PublicationsManager : IPublicationsManager
    {
        private readonly IPublicationsService _publicationsService;

        public PublicationsManager(IPublicationsService publicationsService)
        {
            _publicationsService = publicationsService;
        }

        public async Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            return await _publicationsService.GetPopularVideoFeedAsync(dateFilterType, page, pageSize);
        }

        public async Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            return await _publicationsService.GetActualVideoFeedAsync(dateFilterType, page, pageSize);
        }

        public async Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            return await _publicationsService.GetMyVideoFeedAsync(page, pageSize, dateFilterType);
        }

        public async Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            return await _publicationsService.SendLikeAsync(videoId, isChecked, cancellationToken);
        }

        public async Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            return await _publicationsService.SendDislikeAsync(videoId, isChecked, cancellationToken);
        }
    }
}