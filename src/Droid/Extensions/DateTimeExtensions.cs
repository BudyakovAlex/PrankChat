using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToDialogPickerDate(this DateTime dateTime)
        {
            return (long)(dateTime.Date - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}