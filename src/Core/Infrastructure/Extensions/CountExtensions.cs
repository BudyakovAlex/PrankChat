﻿using System;
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
        private const string DefaultValue = "0";

        public static string ToCountString(this int? count)
        {
            if (count == null)
                return DefaultValue;

            return ((long?)count).ToCountString();
        }

        public static string ToCountString(this long? count)
        {
            if (count == null)
                return DefaultValue;

            if (count >= BigLimitForCount)
                return (count / BigLimitForCount)?.ToString(FormatForCount) + Resources.Count_Millions;

            if (count >= SmallLimitForCount)
                return (count / SmallLimitForCount)?.ToString(FormatForCount) + Resources.Count_Thousand;

            return count?.ToString(FormatForCountWithFraction);
        }

        public static string ToCountViewsString(this long? count)
        {
            var countString = count.ToString();
            var lastTwoCharsString = countString.Substring(Math.Max(0, countString.Length - 2));
            var lastTwoChars = long.Parse(lastTwoCharsString);
            if (lastTwoChars >= 11 && lastTwoChars <= 19)
                return $"{count.ToCountString()} {Resources.Count_Views}";

            var lastChar = count.ToString().LastOrDefault();

            string viewsText;
            if (lastChar == '1')
            {
                viewsText = Resources.Count_View;
            }
            else
            {
                viewsText = new[] { '2', '3', '4' }.Contains(lastChar)
                    ? Resources.Count_Of_Viewing
                    : Resources.Count_Views;
            }

            return $"{count.ToCountString()} {viewsText}";
        }
    }
}
