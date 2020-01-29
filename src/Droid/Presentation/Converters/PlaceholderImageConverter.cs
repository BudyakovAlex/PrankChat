using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class PlaceholderImageConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "res:ic_image_background.png";

            return value;
        }
    }
}
