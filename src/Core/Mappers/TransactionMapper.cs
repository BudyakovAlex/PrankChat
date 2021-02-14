using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class TransactionMapper
    {
        public static Transaction Map(this TransactionDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Transaction(
                dto.Id,
                dto.Amount,
                dto.Comment,
                dto.Direction,
                dto.Reason,
                dto.BalanceBefore,
                dto.BalanceAfter,
                dto.FrozenBefore,
                dto.FrozenAfter,
                dto.User?.Map());
        }

        public static Transaction Map(this ResponseDto<TransactionDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Transaction(
                dto.Data.Id,
                dto.Data.Amount,
                dto.Data.Comment,
                dto.Data.Direction,
                dto.Data.Reason,
                dto.Data.BalanceBefore,
                dto.Data.BalanceAfter,
                dto.Data.FrozenBefore,
                dto.Data.FrozenAfter,
                dto.Data.User?.Map());
        }
    }
}