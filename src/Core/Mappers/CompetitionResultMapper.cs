using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionResultMapper
    {
        public static CompetitionResultDataModel Map(this CompetitionResultApiModel competitionResultApiModel)
        {
            return new CompetitionResultDataModel(competitionResultApiModel.Place,
                                          competitionResultApiModel.User.Map(),
                                          competitionResultApiModel.Video.Data.Map(),
                                          competitionResultApiModel.Prize);
        }
    }
}
