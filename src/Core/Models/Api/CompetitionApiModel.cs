using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class CompetitionApiModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PrizePool { get; set; }

        public int LikesCount { get; set; }

        public DateTime VoteTerm { get; set; }

        public DateTime NewTerm { get; set; }
    }
}
