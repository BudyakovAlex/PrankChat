using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class WithdrawalMapper
    {
        public static Withdrawal Map(this WithdrawalDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Withdrawal(
                dto.Id,
                dto.Amount,
                dto.Status,
                dto.CreatedAt);
        }

        public static Withdrawal Map(this ResponseDto<WithdrawalDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Withdrawal(
                dto.Data.Id,
                dto.Data.Amount,
                dto.Data.Status,
                dto.Data.CreatedAt);
        }
    }
}