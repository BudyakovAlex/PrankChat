using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Search
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchService _searchService;

        public SearchManager(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public Task<PaginationModel<VideoDataModel>> SearchVideosAsync(string query, int page, int pageSize)
        {
            return _searchService.SearchVideosAsync(query, page, pageSize);
        }

        public Task<PaginationModel<UserDataModel>> SearchUsersAsync(string query, int page, int pageSize)
        {
            return _searchService.SearchUsersAsync(query, page, pageSize);
        }

        public Task<PaginationModel<OrderDataModel>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            return _searchService.SearchOrdersAsync(query, page, pageSize);
        }
    }
}