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

        [JsonProperty(PropertyName = "views_count")]
        public long ViewsCount { get; set; }

        [JsonProperty(PropertyName = "likes_count")]
        public long LikesCount { get; set; }

        [JsonProperty(PropertyName = "reposts_count")]
        public long RepostsCount { get; set; }

        [JsonProperty(PropertyName = "stream_url")]
        public string StreamUri { get; set; }

        [JsonProperty(PropertyName = "share_url")]
        public string ShareUri { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        public Dictionary<string, UserApiModel> User { get; set; }
    }
}
