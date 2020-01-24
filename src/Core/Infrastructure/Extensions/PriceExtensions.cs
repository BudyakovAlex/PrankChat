using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class PriceExtensions
    {
        private const string FormatForPrice = "#,#.#";
        private const string DefaultValue = "0";

        public static string ToPriceString(this double? price)
        {
            if (price == null)
                return GetStringWithCurrency(DefaultValue);


            return price?.ToString(GetStringWithCurrency(FormatForPrice)).Replace(',', ' ');
        }

        private static string GetStringWithCurrency(string value)
        {
            return $"{value} {Resources.Currency}";
        }
    }
}
