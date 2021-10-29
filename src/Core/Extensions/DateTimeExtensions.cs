using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Localization;
using System;
using System.Globalization;

namespace PrankChat.Mobile.Core.Extensions
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
                    afterCount = Resources.WeeksShort;
                }
                else
                {
                    afterCount = Resources.DaysShort;
                }
            }
            else if (date.Hours != 0)
            {
                count = date.Hours;
                afterCount = Resources.HoursShort;
            }
            else if (date.Minutes != 0)
            {
                count = date.Minutes;
                afterCount = Resources.MinutesShort;
            }
            else
            {
                count = date.Seconds;
                afterCount = Resources.SecondsShort;
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
                return $"{dateTime.ToString(Constants.Formats.DateMoreSevenDays, CultureInfo.CurrentCulture)} {Resources.YearShort}";
            else
                return dateTime.ToString(Constants.Formats.DateLessSevenDays, CultureInfo.CurrentCulture);
        }
    }
}
