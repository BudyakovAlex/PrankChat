using System;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Core.Converters
{
    public class StringFormatValueConverter : MvxValueConverter
    {
        public const string Name = "StringFormat";

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter == null)
            {
                return value;
            }

            var format = "{0:" + parameter.ToString() + "}";
            return string.Format(format, value);
        }
    }
}