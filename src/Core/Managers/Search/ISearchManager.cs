using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Search
{
    public interface ISearchManager
    {
        Task<Pagination<Models.Data.Video>> SearchVideosAsync(string query, int page, int pageSize);

        Task<Pagination<User>> SearchUsersAsync(string query, int page, int pageSize);

        Task<Pagination<Order>> SearchOrdersAsync(string query, int page, int pageSize);
    }
}