using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class VideoApiModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        [JsonProperty("poster")]
        public string Poster { get; set; }

        [JsonProperty("views_count")]
        public long? ViewsCount { get; set; }

        [JsonProperty("likes_count")]
        public long? LikesCount { get; set; }

        [JsonProperty("reposts_count")]
        public long? RepostsCount { get; set; }

        [JsonProperty("stream_url")]
        public string StreamUri { get; set; }

        [JsonProperty("marked_stream_url")]
        public string MarkedStreamUri { get; set; }

        [JsonProperty("share_url")]
        public string ShareUri { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("is_liked")]
        public bool IsLiked { get; set; }

        public DataApiModel<UserApiModel> User { get; set; }
    }
}
