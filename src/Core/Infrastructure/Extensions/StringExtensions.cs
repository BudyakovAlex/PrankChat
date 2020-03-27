using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using CreditCardValidator;

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

        public static string ToShortenName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            if (name.Length < ShortenCountForname)
                return name;

            return name.Substring(0, ShortenCountForname).ToUpper();
        }

        public static bool IsDigit(this char source)
        {
            return source >= '0' && source <= '9';
        }
    }
}
