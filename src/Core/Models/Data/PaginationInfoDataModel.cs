using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PaginationInfoDataModel
    {
        public PaginationInfoDataModel(long total,
                                       long count,
                                       int perPage,
                                       long currentPage,
                                       long totalPages,
                                       Dictionary<string, string> links)
        {
            Total = total;
            Count = count;
            PerPage = perPage;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            Links = links;
        }

        public long Total { get; set; }

        public long Count { get; set; }

        public int PerPage { get; set; }

        public long CurrentPage { get; set; }

        public long TotalPages { get; set; }

        public Dictionary<string, string> Links { get; set; }
    }
}
