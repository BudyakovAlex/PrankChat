using PrankChat.Mobile.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class Competition
    {
        public Competition(
            int id,
            string title,
            string imageUrl,
            string description,
            string htmlContent,
            OrderCategory? category,
            CompetitionStatus status,
            bool canUploadVideo,
            bool canDelete,
            string shareUrl,
            bool isCompetitionOwner,
            int prizePool,
            int? likesCount,
            int? videosCount,
            decimal? entryTax,
            bool isPaidCompetitionMember,
            bool canJoin,
            List<string> prizePoolList,
            DateTime? voteTo,
            DateTime? uploadVideoTo,
            DateTime? createdAt,
            DateTime? activeTo,
            User? customer)
        {
            Id = id;
            Title = title;
            ImageUrl = imageUrl;
            Description = description;
            HtmlContent = htmlContent;
            Category = category;
            Status = status;
            CanUploadVideo = canUploadVideo;
            CanDelete = canDelete;
            ShareUrl = shareUrl;
            IsCompetitionOwner = isCompetitionOwner;
            PrizePool = prizePool;
            LikesCount = likesCount;
            VideosCount = videosCount;
            EntryTax = entryTax;
            IsPaidCompetitionMember = isPaidCompetitionMember;
            CanJoin = canJoin;
            PrizePoolList = prizePoolList;
            VoteTo = voteTo;
            UploadVideoTo = uploadVideoTo;
            CreatedAt = createdAt;
            ActiveTo = activeTo;
            Customer = customer;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string HtmlContent { get; set; }

        public OrderCategory? Category { get; set; }

        public CompetitionStatus Status { get; set; }

        public bool CanUploadVideo { get; set; }

        public bool CanDelete { get; }

        public string ShareUrl { get; }

        public bool IsPaidCompetitionMember { get; set; }

        public bool IsCompetitionOwner { get; }

        public bool CanJoin { get; set; }

        public int PrizePool { get; set; }

        public int? LikesCount { get; set; }

        public int? VideosCount { get; set; }

        public decimal? EntryTax { get; }

        public List<string> PrizePoolList { get; set; }

        public DateTime? VoteTo { get; set; }

        public DateTime? UploadVideoTo { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ActiveTo { get; set; }

        public User? Customer { get; }
    }
}