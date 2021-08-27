using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class BooleanToSoundResourceConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? "ImagePathProvider" : "ic_without_sound";
        }
    }
}
