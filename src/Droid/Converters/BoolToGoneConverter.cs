using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Converters
{
    public class BoolToGoneConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}