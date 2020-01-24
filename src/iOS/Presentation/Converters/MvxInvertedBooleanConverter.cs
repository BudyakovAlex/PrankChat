using System;
using System.Globalization;
using MvvmCross.Converters;

namespace PrankChat.Mobile.iOS.Presentation.Converters
{
    public class MvxInvertedBooleanConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }

        protected override bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
