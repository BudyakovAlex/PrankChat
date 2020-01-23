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
        private const string DefaultValueIfNull = "0";

        public static string ToCountString(this int? count)
        {
            if (count.HasValue)
                return count.Value.ToCountString();
            return DefaultValueIfNull;
        }

        public static string ToCountString(this int count)
        {
            return ((long)count).ToCountString();
        }

        public static string ToCountString(this long count)
        {
            if (count >= BigLimitForCount)
                return (count / BigLimitForCount).ToString(FormatForCount) + Resources.Count_Millions;
            if (count >= SmallLimitForCount)
                return (count / SmallLimitForCount).ToString(FormatForCount) + Resources.Count_Thousand;
            return count.ToString(FormatForCountWithFraction);
        }

        public static string ToCountViewsString(this long? count)
        {
            if (count.HasValue)
                return count.Value.ToCountViewsString();
            return DefaultValueIfNull;
        }

        public static string ToCountViewsString(this long count)
        {
            var countString = count.ToString();
            var lastTwoChars = long.Parse(countString.Substring(Math.Max(0, countString.Length - 2)));
            if (lastTwoChars >= 11 && lastTwoChars <= 19)
                return $"{count.ToCountString()} {Resources.Count_Views}";
            var lastChar = count.ToString().LastOrDefault();
            return count.ToCountString() + " " + (lastChar == '1' ? Resources.Count_View :
                                                   new[] { '2', '3', '4' }.Contains(lastChar) ? Resources.Count_Of_Viewing : Resources.Count_Views);
        }
    }
}
