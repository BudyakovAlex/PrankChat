using System.Linq;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class CountExtensions
    {
        public static string ToUICountString(this int count)
        {
            return ((long)count).ToUICountString();
        }

        public static string ToUICountString(this long count)
        {
            if (count >= 1000000)
                return (count / 1000000D).ToString("0.#") + Resources.count_millions;
            if (count >= 1000)
                return (count / 1000D).ToString("0.#") + Resources.count_thousand;
            return count.ToString("#,0");
        }

        public static string ToUICountViewsString(this long count)
        {
            char lastChar = count.ToString().Last();
            return count.ToUICountString() + " " + (lastChar == '1' ? Resources.count_view :
                                                           new[] { '2', '3', '4' }.Contains(lastChar) ? Resources.count_of_viewing : Resources.count_views);
        }
    }
}
