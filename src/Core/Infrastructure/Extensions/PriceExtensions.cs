namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class PriceExtensions
    {
        public static string ToPriceUIString(this double price)
        {
            return price.ToString("#,#.# ₽").Replace(',', ' ');
        }
    }
}
