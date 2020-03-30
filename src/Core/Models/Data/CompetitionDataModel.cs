using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CompetitionDataModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string HtmlContent { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public int PrizePool { get; set; }

        public int? LikesCount { get; set; }

        public int? VideosCount { get; set; }

        public List<string> PrizePoolList { get; set; }

        public DateTime? VoteTo { get; set; }

        public DateTime UploadVideoTo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ActiveTo { get; set; }
    }
}