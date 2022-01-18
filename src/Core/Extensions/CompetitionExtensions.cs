using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class CompetitionExtensions
    {
        public static CompetitionPhase GetPhase(this Competition competition) => competition.Status switch
        {
            CompetitionStatus.Finished => CompetitionPhase.Finished,
            CompetitionStatus.Moderation => CompetitionPhase.Moderation,
            CompetitionStatus.Voting => CompetitionPhase.Voting,
            _ => CompetitionPhase.New,
        };
    }
}