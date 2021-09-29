using System;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Extensions
{
    // TODO: Move to JSON converter.
    public static class DateFilterExtensions
    {
        private const int DaysInWeek = 7;
        private const int MonthsInQuarter = 3;
        private const int MonthsInHalfYear = 6;
        private const int MonthsInYear = 12;

        public static string GetDateString(this DateFilterType dateFilterType)
        {
            var days = GetDays(dateFilterType);
            return (DateTime.UtcNow - TimeSpan.FromDays(days)).ToString(DateFormats.RestApiDate);
        }

        private static int GetDays(DateFilterType dateFilterType) => dateFilterType switch
        {
            DateFilterType.Day => 0,
            DateFilterType.Week => DaysInWeek,
            DateFilterType.Month => GetDaysInMonth(),
            DateFilterType.Quarter => GetDaysInMonth(MonthsInQuarter),
            DateFilterType.HalfYear => GetDaysInMonth(MonthsInHalfYear),
            DateFilterType.Year => GetDaysInMonth(MonthsInYear),
            DateFilterType.All => int.MaxValue,
            _ => 0,
        };

        private static int GetDaysInMonth(int monthAgo = 0)
        {
            var days = 0;
            days += DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
            for (var i = 1; i < monthAgo; i++)
            {
                var timePeriod = DateTime.UtcNow - TimeSpan.FromDays(days);
                days += DateTime.DaysInMonth(timePeriod.Year, timePeriod.Month);
            }

            return days;
        }
    }
}
