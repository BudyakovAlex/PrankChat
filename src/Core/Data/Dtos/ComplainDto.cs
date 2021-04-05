using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ComplainDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
