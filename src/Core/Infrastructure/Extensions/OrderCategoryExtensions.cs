using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class OrderCategoryExtensions
    {
        public static bool CheckIsCompetitionOrder(this OrderCategory? orderCategory)
        {
            switch (orderCategory)
            {
                case OrderCategory.Competition:
                case OrderCategory.PaidCompetition:
                case OrderCategory.PrivatePaidCompetition:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CheckIsPaidCompetitionOrder(this OrderCategory? orderCategory)
        {
            switch (orderCategory)
            {
                case OrderCategory.PaidCompetition:
                case OrderCategory.PrivatePaidCompetition:
                    return true;
                default:
                    return false;
            }
        }
    }
}
