using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class OrderCategoryExtensions
    {
        public static bool CheckIsCompetitionOrder(this OrderCategory? orderCategory)
        {
            return orderCategory switch
            {
                OrderCategory.Competition => true,
                OrderCategory.PaidCompetition => true,
                OrderCategory.PrivatePaidCompetition => true,
                _ => false,
            };
        }

        public static bool CheckIsPaidCompetitionOrder(this OrderCategory? orderCategory)
        {
            return orderCategory switch
            {
                OrderCategory.PaidCompetition => true,
                OrderCategory.PrivatePaidCompetition => true,
                _ => false,
            };
        }
    }
}
