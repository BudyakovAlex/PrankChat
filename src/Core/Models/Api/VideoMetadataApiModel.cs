using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class VideoMetadataApiModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        [JsonProperty("views_count")]
        public long? ViewsCount { get; set; }

        [JsonProperty("likes_count")]
        public long? LikesCount { get; set; }

        [JsonProperty("reposts_count")]
        public long? RepostsCount { get; set; }

        [JsonProperty("stream_url")]
        public string StreamUri { get; set; }

        [JsonProperty("share_url")]
        public string ShareUri { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("is_liked")]
        public bool IsLiked { get; set; }

        public Dictionary<string, UserApiModel> User { get; set; }
    }
}
