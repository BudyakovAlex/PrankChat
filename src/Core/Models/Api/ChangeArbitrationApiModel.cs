using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class ChangeArbitrationApiModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
