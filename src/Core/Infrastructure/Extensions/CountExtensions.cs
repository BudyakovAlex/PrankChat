using System;
using System.Linq;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class CountExtensions
    {
        private const string FormatForCount = "0.#";
        private const string FormatForCountWithFraction = "#,0";
        private const double BigLimitForCount = 1000000D;
        private const double SmallLimitForCount = 1000D;

        public static string ToCountString(this int count)
        {
            return ((long)count).ToCountString();
        }

        public static string ToCountString(this long count)
        {
            if (count >= BigLimitForCount)
                return (count / BigLimitForCount).ToString(FormatForCount) + Resources.count_millions;
            if (count >= SmallLimitForCount)
                return (count / SmallLimitForCount).ToString(FormatForCount) + Resources.count_thousand;
            return count.ToString(FormatForCountWithFraction);
        }

        public static string ToCountViewsString(this long count)
        {
            char lastChar = count.ToString().LastOrDefault();
            return count.ToCountString() + " " + (lastChar == '1' ? Resources.count_view :
                                                   new[] { '2', '3', '4' }.Contains(lastChar) ? Resources.count_of_viewing : Resources.count_views);
        }
    }
}
