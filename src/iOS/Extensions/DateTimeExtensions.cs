using System;
using Foundation;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class DateTimeExtensions
    {
        public static NSDate? ToNSDate(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return MvvmCross.Platforms.Ios.MvxIosDateTimeExtensions.ToNSDate((DateTime)dateTime);
        }
    }
}
