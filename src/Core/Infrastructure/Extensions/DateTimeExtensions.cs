using System;
using System.IO.Compression;
using System.Linq;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToUITimeAgoString(this DateTime dateTime)
        {
            var date = DateTime.UtcNow - dateTime;
            string afterCount;
            int count;
            if (date.Days != 0)
            {
                count = date.Days;
                if (count >= 7)
                {
                    count /= 7;
                    afterCount = Resources.weeks_short;
                }
                else
                    afterCount = Resources.days_short;
            }
            else if (date.Hours != 0)
            {
                count = date.Hours;
                afterCount = Resources.hours_short;
            }
            else if (date.Minutes != 0)
            {
                count = date.Minutes;
                afterCount = Resources.minutes_short;
            }
            else
            {
                count = date.Seconds;
                afterCount = Resources.seconds_short;
            }

            return count + afterCount;
        }

        public static string ToUITimeWithSpaceString(this DateTime dateTime)
        {
            return dateTime.ToString("dd : hh : mm");
        }
    }
}
