using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Services.Network.Http.Publications;
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

        public async Task<Pagination<Models.Data.Video>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var response = await _publicationsService.GetPopularVideoFeedAsync(dateFilterType, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Models.Data.Video>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var response = await _publicationsService.GetActualVideoFeedAsync(dateFilterType, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Models.Data.Video>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            var response = await _publicationsService.GetMyVideoFeedAsync(page, pageSize, dateFilterType);
            return response.Map(item => item.Map());
        }
    }
}