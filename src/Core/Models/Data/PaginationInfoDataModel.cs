using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class PaginationInfoDataModel
    {
        public long Total { get; set; }

        public long Count { get; set; }

        public int PerPage { get; set; }

        public long CurrentPage { get; set; }

        public long TotalPages { get; set; }

        public Dictionary<string, string> Links { get; set; }
    }
}
