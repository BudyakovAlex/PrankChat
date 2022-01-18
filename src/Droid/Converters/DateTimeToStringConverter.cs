using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PrankChat.Mobile.Droid.Converters
{
    public class DateTimeToStringConverter : MvxValueConverter<DateTime?, string>
    {
        protected override string Convert(DateTime? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToShortDateString();
        }

        protected override DateTime? ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime.TryParse(value, out var result);
            return result;
        }
    }
}