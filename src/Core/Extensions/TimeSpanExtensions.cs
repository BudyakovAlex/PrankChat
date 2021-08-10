using PrankChat.Mobile.Core.Common.Constants;
using System;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToTimeWithSpaceString(this TimeSpan value)
        {
            return value.ToString(Constants.Formats.DateWithSpace);
        }
    }
}
