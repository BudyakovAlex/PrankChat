using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class UserRegistrationApiModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonProperty("password_confirmation")]
        public string PasswordConfirmation { get; set; }
    }
}
