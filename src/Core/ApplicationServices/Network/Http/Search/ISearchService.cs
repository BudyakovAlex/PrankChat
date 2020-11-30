using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search
{
    public interface ISearchService
    {
        Task<BaseBundleApiModel<VideoApiModel>> SearchVideosAsync(string query, int page, int pageSize);

        Task<BaseBundleApiModel<UserApiModel>> SearchUsersAsync(string query, int page, int pageSize);

        Task<BaseBundleApiModel<OrderApiModel>> SearchOrdersAsync(string query, int page, int pageSize);
    }
}