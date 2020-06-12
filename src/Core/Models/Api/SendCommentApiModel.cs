using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class SendCommentApiModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
