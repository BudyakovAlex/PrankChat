using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class PlaceholderImageConverter : MvxValueConverter<string, int>
    {
        protected override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Resource.Drawable.ic_image_background;

            return 0;
        }
    }
}
