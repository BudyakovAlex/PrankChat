using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionResultMapper
    {
        public static CompetitionResultDataModel Map(this CompetitionResultApiModel competitionResultApiModel)
        {
            if (competitionResultApiModel.User.Data is null ||
                competitionResultApiModel.Video.Data is null)
            {
                return null;
            }

            return new CompetitionResultDataModel(competitionResultApiModel.Place,
                                                  competitionResultApiModel.User?.Map(),
                                                  competitionResultApiModel.Video.Data?.Map(),
                                                  competitionResultApiModel.Prize);
        }
    }
}