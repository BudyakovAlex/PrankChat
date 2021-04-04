using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Publications
{
    public interface IPublicationsManager
    {
        Task<Pagination<Models.Data.Video>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<Pagination<Models.Data.Video>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<Pagination<Models.Data.Video>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null);
    }
}