using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;

namespace PrankChat.Mobile.Droid.Converters
{
    public class PercentConvertor : MvxValueConverter<double?, string>
    {
        protected override string Convert(double? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == 0)
            {
                return string.Empty;
            }

            return $"{value} %"; //TODO replace with constant percent
        }

        protected override double? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            value = value?.Replace("%", string.Empty).Replace(" ", string.Empty);
            double.TryParse(value, out var result);
            return result;
        }
    }
}