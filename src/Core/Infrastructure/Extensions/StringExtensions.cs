﻿using System.Text.RegularExpressions;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private const string EmailMask = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private const int ShortenCountForname = 2;

        public static bool IsValidEmail(this string emailaddress)
        {
            var regex = new Regex(EmailMask);
            var match = regex.Match(emailaddress);
            return match.Success;
        }

        public static bool IsNullOrEmpty(this string value) =>
            string.IsNullOrEmpty(value);

        public static bool IsNotNullNorEmpty(this string value) =>
            !IsNullOrEmpty(value);

        public static string ToShortenName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            if (name.Length < ShortenCountForname)
            {
                return name;
            }

            return name.Substring(0, ShortenCountForname).ToUpper();
        }

        public static string WithoutSpace(this string source)
        {
            return source.Replace(" ", string.Empty);
        }

        public static int DigitCount(this string source)
        {
            var digitCount = 0;
            foreach (char ch in source)
            {
                if (ch.IsDigit())
                {
                    digitCount++;
                }
            }

            return digitCount;
        }

        public static bool IsDigit(this char source)
        {
            return source >= '0' && source <= '9';
        }

        /// <summary>
        /// Fixes all non numeric chars
        /// </summary>
        public static string StripAllNonNumericChars(this string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                return target;
            }

            const string pattern = @"[\D\+]+";
            target = Regex.Replace(target, pattern, string.Empty, RegexOptions.Singleline);
            return target;
        }
    }
}
