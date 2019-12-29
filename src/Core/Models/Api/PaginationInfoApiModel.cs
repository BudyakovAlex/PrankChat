using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class PaginationInfoApiModel
    {
        public long Total { get; set; }

        public long Count { get; set; }

        [JsonProperty(PropertyName = "per_page")]
        public int PerPage { get; set; }

        [JsonProperty(PropertyName = "current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty(PropertyName = "total_pages")]
        public long TotalPages { get; set; }

        public Dictionary<string, string> Links { get; set; }
    }
}
