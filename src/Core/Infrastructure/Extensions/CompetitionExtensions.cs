using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class CompetitionExtensions
    {
        public static CompetitionPhase GetPhase(this CompetitionDataModel competition)
        {
            switch (competition.Status)
            {
                case Constants.CompetitionStatuses.Finished:
                    return CompetitionPhase.Finished;
                case Constants.CompetitionStatuses.Voting:
                    return CompetitionPhase.Voting;
                default:
                    return CompetitionPhase.New;
            }
        }
    }
}