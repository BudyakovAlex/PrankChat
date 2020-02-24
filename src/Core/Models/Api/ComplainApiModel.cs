using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class ComplainApiModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
