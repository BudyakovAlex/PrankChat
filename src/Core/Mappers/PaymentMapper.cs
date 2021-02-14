using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class PaymentMapper
    {
        public static Payment Map(this PaymentDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Payment(
                dto.Id,
                dto.Amount,
                dto.Provider,
                dto.Status,
                dto.PaymentLink);
        }

        public static Payment Map(this ResponseDto<PaymentDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Payment(
                dto.Data.Id,
                dto.Data.Amount,
                dto.Data.Provider,
                dto.Data.Status,
                dto.Data.PaymentLink);
        }
    }
}