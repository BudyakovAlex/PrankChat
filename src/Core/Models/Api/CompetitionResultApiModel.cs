using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CompetitionResultApiModel
    {
        [JsonProperty("place")]
        public int Place { get; set; }

        [JsonProperty("user")]
        public UserApiModel User { get; set; }

        [JsonProperty("video")]
        public VideoApiModel Video { get; set; }

        [JsonProperty("prize")]
        public string Prize { get; set; }
    }
}