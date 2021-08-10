using PrankChat.Mobile.Core.Common.Constants;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class CompetitionExtensions
    {
        public static CompetitionPhase GetPhase(this Competition competition)
        {
            return competition.Status switch
            {
                Constants.CompetitionStatuses.Finished => CompetitionPhase.Finished,
                Constants.CompetitionStatuses.Voting => CompetitionPhase.Voting,
                _ => CompetitionPhase.New,
            };
        }
    }
}