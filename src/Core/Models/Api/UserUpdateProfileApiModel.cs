using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

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
        public GenderType Sex { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
