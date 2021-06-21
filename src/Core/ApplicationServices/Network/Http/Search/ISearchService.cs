using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search
{
    public interface ISearchService
    {
        Task<BaseBundleDto<VideoDto>> SearchVideosAsync(string query, int page, int pageSize);

        Task<BaseBundleDto<UserDto>> SearchUsersAsync(string query, int page, int pageSize);

        Task<BaseBundleDto<OrderDto>> SearchOrdersAsync(string query, int page, int pageSize);
    }
}