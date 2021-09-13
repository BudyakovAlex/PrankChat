using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Data;
using System;

namespace PrankChat.Mobile.Core.Extensions.DateFilter
{
    public static class DateFilterTypeExtensions
    {
        public static string ToLocalizedString(this DateFilterType dateFilterType) => dateFilterType switch
        {
            DateFilterType.Day => Resources.PerDay,
            DateFilterType.Week => Resources.PerWeek,
            DateFilterType.Month => Resources.PerMonth,
            DateFilterType.Quarter => Resources.PerQuarter,
            DateFilterType.HalfYear => Resources.PerHalfYear,
            _ => throw new NotSupportedException($"Not supported dataFilterType for value {dateFilterType}"),
        };
    }
}