using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class NotificationMapper
    {
        public static Notification Map(this NotificationDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Notification(
                dto.Id,
                dto.Title,
                dto.Description,
                dto.Text,
                dto.IsDelivered,
                dto.Type,
                dto.CreatedAt,
                dto.RelatedUser?.Map(),
                dto.RelatedOrder?.Map(),
                dto.RelatedVideo?.Map(),
                dto.RelatedTransaction?.Map());
        }
    }
}