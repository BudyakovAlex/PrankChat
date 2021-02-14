using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class SendCommentDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
