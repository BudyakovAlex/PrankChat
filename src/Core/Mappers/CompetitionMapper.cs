using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System;
using System.Linq;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CompetitionMapper
    {
        public static Competition Map(this CompetitionDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            var values = Enum.GetValues(typeof(OrderCategory)).OfType<OrderCategory>();
            var matchedCategory = values.FirstOrDefault(item => item.GetEnumMemberAttrValue() == dto.Category);

            return new Competition(
                dto.Id,
                dto.Title,
                dto.ImageUrl,
                dto.Description,
                dto.HtmlContent,
                matchedCategory,
                dto.Status,
                dto.CanUploadVideo,
                dto.PrizePool,
                dto.LikesCount,
                dto.VideosCount,
                dto.EntryTax,
                dto.IsPaidCompetitionMember,
                dto.CanJoin,
                dto.PrizePoolList,
                dto.VoteTo,
                dto.UploadVideoTo,
                dto.CreatedAt,
                dto.ActiveTo);
        }
    }
}