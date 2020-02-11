using System;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToTimeWithSpaceString(this TimeSpan value)
        {
            return value.ToString(Constants.Formats.DateWithSpace);
        }
    }
}
