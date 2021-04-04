using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using System;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions.DateFilter
{
    public static class DateFilterTypeExtensions
    {
        public static string ToLocalizedString(this DateFilterType dateFilterType)
        {
            return dateFilterType switch
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
}