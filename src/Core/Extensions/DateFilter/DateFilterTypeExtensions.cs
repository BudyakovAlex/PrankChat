using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Data;
using System;

namespace PrankChat.Mobile.Core.Extensions.DateFilter
{
    public static class DateFilterTypeExtensions
    {
        public static string ToLocalizedString(this DateFilterType dateFilterType)
        {
            return dateFilterType switch
            {
                DateFilterType.Day => Resources.PublicationTabFilterDay,
                DateFilterType.Week => Resources.PublicationTabFilterWeek,
                DateFilterType.Month => Resources.PublicationTabFilterMonth,
                DateFilterType.Quarter => Resources.PublicationTabFilterQuarter,
                DateFilterType.HalfYear => Resources.PublicationTabFilterHalfYear,
                _ => throw new NotSupportedException($"Not supported dataFilterType for value {dateFilterType}"),
            };
        }
    }
}