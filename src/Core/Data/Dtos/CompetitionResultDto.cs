using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CompetitionResultDto
    {
        [JsonProperty("place")]
        public int Place { get; set; }

        [JsonProperty("user")]
        public ResponseDto<UserDto> User { get; set; }

        [JsonProperty("video")]
        public ResponseDto<VideoDto> Video { get; set; }

        [JsonProperty("prize")]
        public string Prize { get; set; }
    }
}