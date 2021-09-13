using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Converters
{
    public class InvisibleConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? ViewStates.Visible : ViewStates.Invisible;
            }

            return value == null ? ViewStates.Invisible : ViewStates.Visible;
        }
    }
}
