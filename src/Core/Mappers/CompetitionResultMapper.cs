using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionResultMapper
    {
        public static CompetitionResult Map(this CompetitionResultDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new CompetitionResult(
                dto.Place,
                dto.User?.Map(),
                dto.Video?.Data?.Map(),
                dto.Prize);
        }
    }
}