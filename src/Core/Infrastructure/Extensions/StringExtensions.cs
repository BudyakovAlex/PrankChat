using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        private const string EmailMask = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        public static bool IsValidEmail(this string emailaddress)
        {
            var regex = new Regex(EmailMask);
            var match = regex.Match(emailaddress);
            return match.Success;
        }
    }
}
