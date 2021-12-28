using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Models;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionStatisticsMapper
    {
        public static CompetitionStatistics Map(this CompetitionStatisticsDto dto) =>
            new CompetitionStatistics(
                dto.Profit,
                dto.Participants,
                dto.Contribution,
                dto.PercentageFromContribution);
    }
}
