using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.iOS.Converters
{
    public class DateFormatConverter : MvxValueConverter<DateTime?, string>
    {
        // todo move to VM
        protected override string Convert(DateTime? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.HasValue)
                return string.Empty;

            if (parameter == null)
                return string.Empty;

            if (parameter is string format)
            {
                var formatMask = "{0:" + format + "}";
                return string.Format(formatMask, value);
            }

            return string.Empty;
        }
    }
}
