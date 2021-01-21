using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System;
using System.Linq;

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

            var values = Enum.GetValues(typeof(OrderCategory)).OfType<OrderCategory>();
            var matchedCategory = values.FirstOrDefault(item => item.GetEnumMemberAttrValue() == competitionApiModel.Category);

            return new CompetitionDataModel(competitionApiModel.Id,
                                            competitionApiModel.Title,
                                            competitionApiModel.ImageUrl,
                                            competitionApiModel.Description,
                                            competitionApiModel.HtmlContent,
                                            matchedCategory,
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