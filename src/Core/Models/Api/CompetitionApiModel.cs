using System;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CompetitionApiModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string HtmlContent { get; set; }

        public int PrizePool { get; set; }

        public int? LikesCount { get; set; }

        public bool HasLoadedVideo { get; set; }

        public DateTime VoteTerm { get; set; }

        public DateTime NewTerm { get; set; }
    }
}
