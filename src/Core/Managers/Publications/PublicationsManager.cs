using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications;
using PrankChat.Mobile.Core.Mappers;
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
            var response = await _publicationsService.GetPopularVideoFeedAsync(dateFilterType, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var response = await _publicationsService.GetActualVideoFeedAsync(dateFilterType, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            var response = await _publicationsService.GetMyVideoFeedAsync(page, pageSize, dateFilterType);
            return response.Map(item => item.Map());
        }

        public async Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _publicationsService.SendLikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }

        public async Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _publicationsService.SendDislikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }
    }
}