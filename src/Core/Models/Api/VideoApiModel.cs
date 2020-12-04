using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class VideoApiModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        [JsonProperty("poster")]
        public string Poster { get; set; }

        [JsonProperty("views_count")]
        public long? ViewsCount { get; set; }

        [JsonProperty("likes_count")]
        public long? LikesCount { get; set; }

        [JsonProperty("dislikes_count")]
        public long? DislikesCount { get; set; }

        [JsonProperty("comments_count")]
        public long? CommentsCount { get; set; }

        [JsonProperty("reposts_count")]
        public long? RepostsCount { get; set; }

        [JsonProperty("stream_url")]
        public string StreamUri { get; set; }

        [JsonProperty("preview_stream_url")]
        public string PreviewUri { get; set; }

        [JsonProperty("marked_stream_url")]
        public string MarkedStreamUri { get; set; }

        [JsonProperty("share_url")]
        public string ShareUri { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("is_liked")]
        public bool IsLiked { get; set; }

        [JsonProperty("is_disliked")]
        public bool IsDisliked { get; set; }

        [JsonProperty("order_type")]
        public OrderCategory? OrderCategory { get; set; }

        public DataApiModel<UserApiModel> User { get; set; }

        [JsonProperty("customer")]
        public DataApiModel<UserApiModel> Customer { get; set; }
    }
}
