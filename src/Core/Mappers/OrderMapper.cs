using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class OrderMapper
    {
        public static Order Map(this OrderDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Order(
                dto.Id,
                dto.Price,
                dto.Title,
                dto.Description,
                dto.Status,
                dto.OrderCategory,
                dto.ActiveTo,
                dto.DurationInHours,
                dto.AutoProlongation,
                dto.CreatedAt,
                dto.TakenToWorkAt,
                dto.VideoUploadedAt,
                dto.ArbitrationFinishAt,
                dto.CloseOrderAt,
                dto.Customer?.Map(),
                dto.Executor?.Map(),
                dto.Video?.Map(),
                dto.MyArbitrationValue,
                dto.NegativeArbitrationValuesCount,
                dto.PositiveArbitrationValuesCount);
        }

        public static Order Map(this ResponseDto<OrderDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Order(
                dto.Data.Id,
                dto.Data.Price,
                dto.Data.Title,
                dto.Data.Description,
                dto.Data.Status,
                dto.Data.OrderCategory,
                dto.Data.ActiveTo,
                dto.Data.DurationInHours,
                dto.Data.AutoProlongation,
                dto.Data.CreatedAt,
                dto.Data.TakenToWorkAt,
                dto.Data.VideoUploadedAt,
                dto.Data.ArbitrationFinishAt,
                dto.Data.CloseOrderAt,
                dto.Data.Customer?.Map(),
                dto.Data.Executor?.Map(),
                dto.Data.Video?.Map(),
                dto.Data.MyArbitrationValue,
                dto.Data.NegativeArbitrationValuesCount,
                dto.Data.PositiveArbitrationValuesCount);
        }
    }
}