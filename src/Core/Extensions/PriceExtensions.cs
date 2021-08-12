using PrankChat.Mobile.Core.Localization;
using System.Globalization;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class PriceExtensions
    {
        private const string FormatForPrice = "#,#.#";
        private const string DefaultValue = "0";

        public static string ToPriceString(this double? price)
        {
            if (price == null || price == 0)
            {
                return GetStringWithCurrency(DefaultValue);
            }

            return price?.ToString(GetStringWithCurrency(FormatForPrice), CultureInfo.CurrentCulture)
                         .Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                         .Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, " ");
        }

        public static double? PriceToDouble(this string price)
        {
            var value = price?.Replace(Resources.Currency, string.Empty).Replace(" ", string.Empty);
            double.TryParse(value, out var result);
            return result;
        }

        private static string GetStringWithCurrency(string value)
        {
            return $"{value} {Resources.Currency}";
        }
    }
}
