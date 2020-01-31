using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class UserUpdateProfileApiModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
