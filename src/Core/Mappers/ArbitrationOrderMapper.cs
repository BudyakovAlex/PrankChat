using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class ArbitrationOrderMapper
    {
        public static ArbitrationOrder Map(this ArbitrationOrderDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new ArbitrationOrder(
                dto.Id,
                dto.Price,
                dto.Title,
                dto.Description,
                dto.Status,
                dto.AutoProlongation,
                dto.Customer?.Map(),
                dto.Executor?.Map(),
                dto.Video?.Map(),
                dto.Likes,
                dto.Dislikes,
                dto.ArbitrationFinishAt);
        }
    }
}