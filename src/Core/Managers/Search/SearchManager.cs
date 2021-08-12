﻿using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Services.Network.Http.Search;
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

        public async Task<Pagination<Models.Data.Video>> SearchVideosAsync(string query, int page, int pageSize)
        {
            var response = await _searchService.SearchVideosAsync(query, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<User>> SearchUsersAsync(string query, int page, int pageSize)
        {
            var response = await _searchService.SearchUsersAsync(query, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Order>> SearchOrdersAsync(string query, int page, int pageSize)
        {
            var response = await _searchService.SearchOrdersAsync(query, page, pageSize);
            return response.Map(item => item.Map());
        }
    }
}