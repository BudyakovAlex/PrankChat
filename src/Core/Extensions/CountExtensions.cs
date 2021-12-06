using System;
using System.Linq;
using PrankChat.Mobile.Core.Localization;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class CountExtensions
    {
        private const string FormatForCount = "0.#";
        private const string FormatForCountWithFraction = "#,0";
        private const double BigLimitForCount = 1000000D;
        private const double SmallLimitForCount = 1000D;
        private const string DefaultValue = "0";

        private static readonly string[] _weightSuffixes = new[]
        {
            Resources.Bytes,
            Resources.Kb,
            Resources.Mb,
            Resources.Gb,
            Resources.Tb
        };

        public static string ToCountString(this int count)
        {
            return ((long?)count).ToCountString();
        }

        public static string ToCountString(this long count)
        {
            return ((long?)count).ToCountString();
        }

        public static string ToCountString(this int? count)
        {
            if (count == null)
            {
                return DefaultValue;
            }

            return ((long?)count).ToCountString();
        }

        public static string ToCountString(this long? count)
        {
            if (count == null || count == 0)
            {
                return DefaultValue;
            }

            if (count >= BigLimitForCount)
            {
                return (count / BigLimitForCount)?.ToString(FormatForCount) + Resources.CountMillions;
            }

            if (count >= SmallLimitForCount)
            {
                return (count / SmallLimitForCount)?.ToString(FormatForCount) + Resources.CountThousand;
            }

            return count?.ToString(FormatForCountWithFraction) ?? string.Empty;
        }

        public static string ToCountViewsString(this long? count)
        {
            var countString = count.ToString();
            var lastTwoCharsString = countString.Substring(Math.Max(0, countString.Length - 2));
            var lastTwoChars = long.Parse(lastTwoCharsString);
            if (lastTwoChars >= 11 && lastTwoChars <= 19)
            {
                return $"{count.ToCountString()} {Resources.Views}";
            }

            var lastChar = count.ToString().LastOrDefault();
            if (lastChar == '1')
            {
                return $"{count.ToCountString()} {Resources.View}";
            }

            var viewsText = new[] { '2', '3', '4' }.Contains(lastChar)
                  ? Resources.OfViewing
                  : Resources.Views;

            return $"{count.ToCountString()} {viewsText}";
        }

        public static string ToFileSizePresentation(this long length)
        {
            if (length < 0)
            {
                return string.Empty;
            }

            if (length == 0)
            {
                return $"0.0 {_weightSuffixes[0]}";
            }

            var log = (int)Math.Log(length, 1024);
            var ajd = (decimal)(length) / (1L << log * 10);
            return $"{ajd:n1} {_weightSuffixes.ElementAtOrDefault(log)}";
        }
    }
}
