using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ChangeArbitrationDto
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
