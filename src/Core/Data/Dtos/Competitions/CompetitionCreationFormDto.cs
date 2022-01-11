using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Dtos.Competitions
{
    public class CompetitionCreationFormDto
    {
        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start_upload_videos_at")]
        public DateTime? StartUploadVideosDateTime { get; set; }

        [JsonProperty("competition_upload_video_to")]
        public DateTime? EndUploadVideosDateTime { get; set; }

        [JsonProperty("competition_voting_to")]
        public DateTime? VoteToDateTime { get; set; }

        [JsonProperty("prize_pool")]
        public double[] PrizePool { get; set; }

        [JsonProperty("type")]
        public OrderCategory Type { get; set; }

        [JsonProperty("entry_tax")]
        public double? EntryTax { get; set; }

        [JsonProperty("entry_tax_prize_part")]
        public double? EntryTaxPrizePart { get; set; }
    }
}
