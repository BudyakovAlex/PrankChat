using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Converters
{
    public class PlaceholderColorConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "res:ic_image_background.jpg";

            return value;
        }
    }
}
