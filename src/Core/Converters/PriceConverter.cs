using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Converters
{
    public class PriceConverter : MvxValueConverter<double?, string>
    {
        protected override string Convert(double? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == 0)
                return string.Empty;

            return value.ToPriceString();
        }

        protected override double? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length > 1 && !value.Contains(Resources.Currency))
            {
                // Remove the last char (value), when the user delete chars.
                value = value.Trim();
                value = value.Remove(value.Length - 1, 1);
                return value.PriceToDouble();
            }

            return value.PriceToDouble();
        }
    }
}
