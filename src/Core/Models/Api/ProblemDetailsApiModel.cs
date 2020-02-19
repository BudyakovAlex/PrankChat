using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class ProblemDetailsApiModel
    {
        [JsonProperty("code")]
        public string CodeError { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        [JsonProperty("invalid-params")]
        public List<ItemInvalidParameter> InvalidParams { get; set; }

        [JsonProperty("status")]
        public int? StatusCode { get; set; }

        public string Type { get; set; }
    }
}
