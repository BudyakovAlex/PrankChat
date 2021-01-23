using PrankChat.Mobile.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CompetitionDataModel
    {
        public CompetitionDataModel(int id,
                                    string title,
                                    string imageUrl,
                                    string description,
                                    string htmlContent,
                                    OrderCategory? category,
                                    string status,
                                    bool canUploadVideo,
                                    int prizePool,
                                    int? likesCount,
                                    int? videosCount,
                                    bool isPaidCompetitionMember,
                                    List<string> prizePoolList,
                                    DateTime? voteTo,
                                    DateTime? uploadVideoTo,
                                    DateTime? createdAt,
                                    DateTime? activeTo)
        {
            Id = id;
            Title = title;
            ImageUrl = imageUrl;
            Description = description;
            HtmlContent = htmlContent;
            Category = category;
            Status = status;
            CanUploadVideo = canUploadVideo;
            PrizePool = prizePool;
            LikesCount = likesCount;
            VideosCount = videosCount;
            IsPaidCompetitionMember = isPaidCompetitionMember;
            PrizePoolList = prizePoolList;
            VoteTo = voteTo;
            UploadVideoTo = uploadVideoTo;
            CreatedAt = createdAt;
            ActiveTo = activeTo;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string HtmlContent { get; set; }

        public OrderCategory? Category { get; set; }

        public string Status { get; set; }

        public bool CanUploadVideo { get; set; }

        public bool IsPaidCompetitionMember { get; set; }

        public int PrizePool { get; set; }

        public int? LikesCount { get; set; }

        public int? VideosCount { get; set; }

        public List<string> PrizePoolList { get; set; }

        public DateTime? VoteTo { get; set; }

        public DateTime? UploadVideoTo { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ActiveTo { get; set; }
    }
}