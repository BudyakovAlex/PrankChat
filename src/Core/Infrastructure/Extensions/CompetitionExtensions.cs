using System;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class CompetitionExtensions
    {
        public static CompetitionPhase GetPhase(this CompetitionDataModel competition)
        {
            if (competition.NewTerm > DateTime.UtcNow)
            {
                return CompetitionPhase.New;
            }

            if (competition.VoteTerm > DateTime.UtcNow)
            {
                return CompetitionPhase.Voting;
            }

            return CompetitionPhase.Finished;
        }
    }
}
