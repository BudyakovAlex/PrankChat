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
            int count = 0;
            string afterCount = "";
            if (date.Days != 0)
            {
                count = date.Days;
                if (count >= 30)
                {
                    count /= 30;
                    afterCount = count.GetTrueResource(Resources.one_month_ago, Resources.month_ago, Resources.months_ago);
                }
                else
                    afterCount = count.GetTrueResource(Resources.one_day_ago, Resources.day_ago, Resources.days_ago);
            }
            else if (date.Hours != 0)
            {
                count = date.Hours;
                afterCount = count.GetTrueResource(Resources.one_hour_ago, Resources.hour_ago, Resources.hours_ago);
            }
            else if (date.Minutes != 0)
            {
                count = date.Minutes;
                afterCount = count.GetTrueResource(Resources.one_minute_ago, Resources.minute_ago, Resources.minutes_ago);
            }
            else
            {
                count = date.Seconds;
                afterCount = count.GetTrueResource(Resources.one_second_ago, Resources.second_ago, Resources.seconds_ago);
            }

            return count + " " + afterCount;
        }

        private static string GetTrueResource(this int count, string oneResource, string resource, string resources)
        {
            char lastChar = count.ToString().Last();
            return lastChar == '1' ? oneResource :
                   new[] { '2', '3', '4' }.Contains(lastChar) ? resource : resources;
        }

        public static string ToUITimeWithSpaceString(this DateTime dateTime)
        {
            return dateTime.ToString("dd : hh : mm");
        }
    }
}
