using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class SubscriptionsNavigationParameter
    {
        public SubscriptionsNavigationParameter(
            SubscriptionTabType subscriptionTabType,
            int userId,
            string userName)
        {
            SubscriptionTabType = subscriptionTabType;
            UserId = userId;
            UserName = userName;
        }

        public SubscriptionTabType SubscriptionTabType { get; }

        public int UserId { get; }

        public string UserName { get; }
    }
}
