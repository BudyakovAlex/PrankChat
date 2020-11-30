using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class UserMapper
    {
        public static UserDataModel Map(this UserApiModel userApiModel)
        {
            return new UserDataModel(userApiModel.Id,
                             userApiModel.Avatar,
                             userApiModel.Name,
                             userApiModel.Sex,
                             userApiModel.Birthday,
                             userApiModel.DocumentVerifiedAt,
                             userApiModel.EmailVerifiedAt,
                             userApiModel.Document.Map(),
                             userApiModel.IsSubscribed,
                             userApiModel.Login,
                             userApiModel.Email,
                             userApiModel.Balance,
                             userApiModel.Description,
                             userApiModel.OrdersOwnCount,
                             userApiModel.OrdersExecuteCount,
                             userApiModel.OrdersExecuteFinishedCount,
                             userApiModel.SubscribersCount,
                             userApiModel.SubscriptionsCount);
        }

        public static UserDataModel Map(this DataApiModel<UserApiModel> userApiModel)
        {
            return new UserDataModel(userApiModel.Data.Id,
                             userApiModel.Data.Avatar,
                             userApiModel.Data.Name,
                             userApiModel.Data.Sex,
                             userApiModel.Data.Birthday,
                             userApiModel.Data.DocumentVerifiedAt,
                             userApiModel.Data.EmailVerifiedAt,
                             userApiModel.Data.Document.Map(),
                             userApiModel.Data.IsSubscribed,
                             userApiModel.Data.Login,
                             userApiModel.Data.Email,
                             userApiModel.Data.Balance,
                             userApiModel.Data.Description,
                             userApiModel.Data.OrdersOwnCount,
                             userApiModel.Data.OrdersExecuteCount,
                             userApiModel.Data.OrdersExecuteFinishedCount,
                             userApiModel.Data.SubscribersCount,
                             userApiModel.Data.SubscriptionsCount);
        }
    }
}