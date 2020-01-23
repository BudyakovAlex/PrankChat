using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class PriceExtensions
    {
        private const string FormatForPrice = "#,#.#";
        private const string DefaultValueIfNull = "0";

        public static string ToPriceString(this double? price)
        {
            if (price.HasValue)
                return price.Value.ToPriceString();
            return GetStringWithCurrency(DefaultValueIfNull);
        }

        public static string ToPriceString(this double price)
        {
            return price.ToString(GetStringWithCurrency(FormatForPrice)).Replace(',', ' ');
        }

        private static string GetStringWithCurrency(string value)
        {
            return $"{value} {Resources.Currency}";
        }
    }
}
