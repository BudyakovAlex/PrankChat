using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Extensions;

namespace PrankChat.Mobile.Core.Converters
{
    public class PriceConverter : MvxValueConverter<double?, string>
    {
        protected override string Convert(double? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == 0)
            {
                return string.Empty;
            }

            return value.ToPriceString();
        }

        protected override double? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.PriceToDouble();
        }
    }
}
