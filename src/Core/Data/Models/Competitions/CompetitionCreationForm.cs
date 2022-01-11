using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Models.Competitions
{
    public class CompetitionCreationForm
    {
        public CompetitionCreationForm(
            double? price,
            string title,
            string description,
            DateTime? startUploadVideosDateTime,
            DateTime? endUploadVideosDateTime,
            DateTime? voteToDateTime,
            double[] prizePool,
            OrderCategory type,
            double? entryTax,
            double? entryTaxPrizePart)
        {
            Price = price;
            Title = title;
            Description = description;
            StartUploadVideosDateTime = startUploadVideosDateTime;
            EndUploadVideosDateTime = endUploadVideosDateTime;
            VoteToDateTime = voteToDateTime;
            PrizePool = prizePool;
            Type = type;
            EntryTax = entryTax;
            EntryTaxPrizePart = entryTaxPrizePart;
        }

        public double? Price { get; }

        public string Title { get; }

        public string Description { get; }

        public DateTime? StartUploadVideosDateTime { get; }

        public DateTime? EndUploadVideosDateTime { get; }

        public DateTime? VoteToDateTime { get; }

        public double[] PrizePool { get; }

        public OrderCategory Type { get; }

        public double? EntryTax { get; }

        public double? EntryTaxPrizePart { get; }
    }
}