using System.Collections.Generic;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ProblemDetailsDto
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
