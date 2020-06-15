using System;
using System.Globalization;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        private const int DaysInWeek = 7;

        public static string ToTimeAgoCommentString(this DateTime dateTime)
        {
            var date = DateTime.UtcNow - dateTime;
            string afterCount;
            int count;
            if (date.Days != 0)
            {
                count = date.Days;
                if (count >= DaysInWeek)
                {
                    count /= DaysInWeek;
                    afterCount = Resources.Weeks_Short;
                }
                else
                {
                    afterCount = Resources.Days_Short;
                }
            }
            else if (date.Hours != 0)
            {
                count = date.Hours;
                afterCount = Resources.Hours_Short;
            }
            else if (date.Minutes != 0)
            {
                count = date.Minutes;
                afterCount = Resources.Minutes_Short;
            }
            else
            {
                count = date.Seconds;
                afterCount = Resources.Seconds_Short;
            }

            return $"{Math.Max(count, 0)}{afterCount}";
        }

        public static string ToTimeWithSpaceString(this DateTime dateTime)
        {
            return dateTime.ToString(Constants.Formats.DateWithSpace);
        }

        public static string ToTimeAgoPublicationString(this DateTime dateTime)
        {
            if ((DateTime.UtcNow - dateTime).Days > DaysInWeek)
                return $"{dateTime.ToString(Constants.Formats.DateMoreSevenDays, CultureInfo.CurrentCulture)} {Resources.Year_Short}";
            else
                return dateTime.ToString(Constants.Formats.DateLessSevenDays, CultureInfo.CurrentCulture);
        }
    }
}
