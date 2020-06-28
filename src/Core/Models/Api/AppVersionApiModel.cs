using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class AppVersionApiModel
    {
        [JsonProperty]
        public string Link { get; set; }
    }
}