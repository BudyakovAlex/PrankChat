using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Core.Converters
{
    public class PercentConverter : MvxValueConverter<double?, string>
    {
        private const string Percent = "%";

        protected override string Convert(double? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == 0)
            {
                return string.Empty;
            }

            return $"{value} {Percent}";
        }

        protected override double? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            value = value?.Replace(Percent, string.Empty).Replace(" ", string.Empty);
            double.TryParse(value, out var result);
            return result;
        }
    }
}
