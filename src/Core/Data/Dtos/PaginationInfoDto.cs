using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class PaginationInfoDto
    {
        public long Total { get; set; }

        public long Count { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }

        public Dictionary<string, string> Links { get; set; }
    }
}
