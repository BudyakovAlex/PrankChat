using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

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

            return new Competition(
                dto.Id,
                dto.Title,
                dto.ImageUrl,
                dto.Description,
                dto.HtmlContent,
                dto.Category,
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
                dto.ActiveTo,
                dto.Customer?.Map());
        }
    }
}