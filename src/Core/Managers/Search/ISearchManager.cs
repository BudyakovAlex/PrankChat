using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Search
{
    public interface ISearchManager
    {
        Task<PaginationModel<VideoDataModel>> SearchVideosAsync(string query, int page, int pageSize);

        Task<PaginationModel<UserDataModel>> SearchUsersAsync(string query, int page, int pageSize);

        Task<PaginationModel<OrderDataModel>> SearchOrdersAsync(string query, int page, int pageSize);
    }
}