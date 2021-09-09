using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Data;
using System;

namespace PrankChat.Mobile.Core.Extensions.DateFilter
{
    public static class DateFilterTypeExtensions
    {
        public static string ToLocalizedString(this DateFilterType dateFilterType) => dateFilterType switch
        {
            DateFilterType.Day => Resources.Publication_Tab_Filter_Day,
            DateFilterType.Week => Resources.Publication_Tab_Filter_Week,
            DateFilterType.Month => Resources.Publication_Tab_Filter_Month,
            DateFilterType.Quarter => Resources.Publication_Tab_Filter_Quarter,
            DateFilterType.HalfYear => Resources.Publication_Tab_Filter_HalfYear,
            _ => throw new NotSupportedException($"Not supported dataFilterType for value {dateFilterType}"),
        };
    }
}