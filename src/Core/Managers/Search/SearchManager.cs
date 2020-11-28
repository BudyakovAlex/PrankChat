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

        public async Task<PaginationModel<VideoDataModel>> SearchVideosAsync(string query, int page, int pageSize)
        {
            return await _searchService.SearchVideosAsync(query, page, pageSize);
        }

        public async Task<PaginationModel<UserDataModel>> SearchUsersAsync(string query, int page, int pageSize)
        {
            return await _searchService.SearchUsersAsync(query, page, pageSize);
        }

        public async Task<PaginationModel<OrderDataModel>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            return await _searchService.SearchOrdersAsync(query, page, pageSize);
        }
    }
}