using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class VideoMapper
    {
        public static Video Map(this VideoDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Video(
                dto.Id,
                dto.Title,
                dto.Description,
                dto.Poster,
                dto.Status,
                dto.ViewsCount ?? 0,
                dto.RepostsCount ?? 0,
                dto.LikesCount ?? 0,
                dto.DislikesCount ?? 0,
                dto.CommentsCount ?? 0,
                dto.StreamUri,
                dto.PreviewUri,
                dto.MarkedStreamUri,
                dto.ShareUri,
                dto.IsLiked,
                dto.IsDisliked,
                dto.OrderCategory,
                dto.CreatedAt,
                dto.User?.Map(),
                dto.Customer?.Map());
        }

        public static Video Map(this ResponseDto<VideoDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Video(
                dto.Data.Id,
                dto.Data.Title,
                dto.Data.Description,
                dto.Data.Poster,
                dto.Data.Status,
                dto.Data.ViewsCount ?? 0,
                dto.Data.RepostsCount ?? 0,
                dto.Data.LikesCount ?? 0,
                dto.Data.DislikesCount ?? 0,
                dto.Data.CommentsCount ?? 0,
                dto.Data.StreamUri,
                dto.Data.PreviewUri,
                dto.Data.MarkedStreamUri,
                dto.Data.ShareUri,
                dto.Data.IsLiked,
                dto.Data.IsDisliked,
                dto.Data.OrderCategory,
                dto.Data.CreatedAt,
                dto.Data.User?.Map(),
                dto.Data.Customer?.Map());
        }
    }
}