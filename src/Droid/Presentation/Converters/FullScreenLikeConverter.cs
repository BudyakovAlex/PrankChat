using MvvmCross.Converters;
using System;
using System.Globalization;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class FullScreenLikeConverter : MvxValueConverter<bool, int>
    {
        protected override int Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? Resource.Drawable.ic_like_active : Resource.Drawable.ic_like_inactive;
        }
    }
}