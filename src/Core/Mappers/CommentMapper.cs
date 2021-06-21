using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CommentMapper
    {
        public static Comment Map(this CommentDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Comment(
                dto.Id,
                dto.Text,
                dto.CreatedAt,
                dto.User?.Map());
        }
    }
}