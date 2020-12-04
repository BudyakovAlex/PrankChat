using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionMapper
    {
        public static CompetitionDataModel Map(this CompetitionApiModel competitionApiModel)
        {
            if (competitionApiModel is null)
            {
                return null;
            }

            return new CompetitionDataModel(competitionApiModel.Id,
                                            competitionApiModel.Title,
                                            competitionApiModel.ImageUrl,
                                            competitionApiModel.Description,
                                            competitionApiModel.HtmlContent,
                                            competitionApiModel.Type,
                                            competitionApiModel.Status,
                                            competitionApiModel.CanUploadVideo,
                                            competitionApiModel.PrizePool,
                                            competitionApiModel.LikesCount,
                                            competitionApiModel.VideosCount,
                                            competitionApiModel.PrizePoolList,
                                            competitionApiModel.VoteTo,
                                            competitionApiModel.UploadVideoTo,
                                            competitionApiModel.CreatedAt,
                                            competitionApiModel.ActiveTo);
        }
    }
}