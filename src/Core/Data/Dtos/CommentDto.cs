using Newtonsoft.Json;
using System;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public string Text { get; set; }

        [JsonProperty("user")]
        public ResponseDto<UserDto> User { get; set; }
    }
}