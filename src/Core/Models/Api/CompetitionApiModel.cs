using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CompetitionApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("poster")]
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("rules")]
        public string HtmlContent { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("prize_pool")]
        public List<string> PrizePoolList { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("price")]
        public int PrizePool { get; set; }

        public int? LikesCount { get; set; }

        [JsonProperty("videos_count")]
        public int? VideosCount { get; set; }

        [JsonProperty("competition_likes_to")]
        public DateTime? VoteTo { get; set; }

        [JsonProperty("competition_upload_video_to")]
        public DateTime UploadVideoTo { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("active_to")]
        public DateTime ActiveTo { get; set; }
    }
}
