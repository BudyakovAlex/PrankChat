using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDataModel Map(this NotificationApiModel notificationApiModel)
        {
            return new NotificationDataModel(notificationApiModel.Id,
                                     notificationApiModel.Title,
                                     notificationApiModel.Description,
                                     notificationApiModel.Text,
                                     notificationApiModel.IsDelivered,
                                     notificationApiModel.Type,
                                     notificationApiModel.CreatedAt,
                                     notificationApiModel.RelatedUser.Map(),
                                     notificationApiModel.RelatedOrder.Map(),
                                     notificationApiModel.RelatedVideo.Map(),
                                     notificationApiModel.RelatedTransaction.Map());
        }
    }
}