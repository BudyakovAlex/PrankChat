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

            return $"{value} {Resources.Percent}"; //TODO replace with constant percent
        }

        protected override double? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value?.Replace(Resources.Percent, string.Empty).Trim();
            double.TryParse(str, out var result);
            return result;
        }
    }
}