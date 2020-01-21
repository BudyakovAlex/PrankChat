using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class PriceExtensions
    {
        private const string FormatForPrice = "#,#.#";

        public static string ToPriceString(this double price)
        {
            return price.ToString($"{FormatForPrice} {Resources.Currency}").Replace(',', ' ');
        }
    }
}
