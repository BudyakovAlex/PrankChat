using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class UserMapper
    {
        public static User Map(this UserDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new User(
                dto.Id,
                dto.Avatar,
                dto.Name,
                dto.Sex,
                dto.Birthday,
                dto.DocumentVerifiedAt,
                dto.EmailVerifiedAt,
                dto.Document?.Map(),
                dto.IsSubscribed,
                dto.Login,
                dto.Email,
                dto.Balance,
                dto.Description,
                dto.OrdersOwnCount,
                dto.OrdersExecuteCount,
                dto.OrdersExecuteFinishedCount,
                dto.SubscribersCount,
                dto.SubscriptionsCount,
                dto.CompetitionsCount);
        }

        public static User Map(this ResponseDto<UserDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new User(
                dto.Data.Id,
                dto.Data.Avatar,
                dto.Data.Name,
                dto.Data.Sex,
                dto.Data.Birthday,
                dto.Data.DocumentVerifiedAt,
                dto.Data.EmailVerifiedAt,
                dto.Data.Document?.Map(),
                dto.Data.IsSubscribed,
                dto.Data.Login,
                dto.Data.Email,
                dto.Data.Balance,
                dto.Data.Description,
                dto.Data.OrdersOwnCount,
                dto.Data.OrdersExecuteCount,
                dto.Data.OrdersExecuteFinishedCount,
                dto.Data.SubscribersCount,
                dto.Data.SubscriptionsCount,
                dto.Data.CompetitionsCount);
        }
    }
}