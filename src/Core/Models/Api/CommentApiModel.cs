using Newtonsoft.Json;
using System;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CommentApiModel
    {
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public string Text { get; set; }

        [JsonProperty("user")]
        public DataApiModel<UserApiModel> User { get; set; }
    }
}