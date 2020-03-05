using System.Globalization;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class PriceExtensions
    {
        private const string FormatForPrice = "#,#.#";
        private const string DefaultValue = "0";

        public static string ToPriceString(this double? price)
        {
            if (price == null || price == 0)
                return GetStringWithCurrency(DefaultValue);

            return price?.ToString(GetStringWithCurrency(FormatForPrice), CultureInfo.CurrentCulture)
                           .Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                           .Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, " ");
        }

        private static string GetStringWithCurrency(string value)
        {
            return $"{value} {Resources.Currency}";
        }
    }
}
