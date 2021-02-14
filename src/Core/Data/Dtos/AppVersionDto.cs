using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class AppVersionDto
    {
        [JsonProperty]
        public string Link { get; set; }
    }
}