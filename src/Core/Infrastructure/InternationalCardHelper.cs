using System.Collections.Generic;
using System.Text.RegularExpressions;
using CreditCardValidator;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Core.Infrastructure
{
    public class InternationalCardHelper
    {
        private static readonly InternationalCardHelper _instance = new InternationalCardHelper();
        public static InternationalCardHelper Instance = _instance;

        public bool IsValidCreditCard(string cardNumber)
        {
            return cardNumber.CreditCardBrandIgnoreLength() != CardIssuer.Unknown;
        }

        public string VisualCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return string.Empty;

            cardNumber = cardNumber.WithoutSpace();
            if (cardNumber.Length <= 4)
                return cardNumber;

            var cardType = cardNumber.CreditCardBrandIgnoreLength();
            var mask = GetCardMask(cardType, cardNumber.Length);

            var pattern = GetPattern(cardNumber.DigitCount());
            var replacement = GetReplacement(mask, cardNumber.DigitCount());
            var formattedNumber = Regex.Replace(cardNumber.StripAllNonNumericChars(), pattern, replacement);
            return formattedNumber;
        }

        private string GetCardMask(CardIssuer cardType, int cardLenght)
        {
            switch (cardType)
            {
                case CardIssuer.Unknown:
                case CardIssuer.Visa:
                case CardIssuer.Switch:
                case CardIssuer.RuPay:
                case CardIssuer.MasterCard:
                case CardIssuer.Laser:
                case CardIssuer.JCB:
                case CardIssuer.Hipercard:
                case CardIssuer.Discover:
                case CardIssuer.Dankort:
                case CardIssuer.AmericanExpress:
                    return "#### #### #### ####";

                case CardIssuer.ChinaUnionPay:
                    if (cardLenght <= 16)
                        return "#### #### #### ####";
                    else
                        return "###### #############";

                case CardIssuer.DinersClub:
                    if (cardLenght <= 14)
                        return "#### ###### ####";
                    else
                        return "#### #### #### ####";

                case CardIssuer.Maestro:
                    if (cardLenght <= 13)
                        return "#### #### #####";

                    if (cardLenght <= 15)
                        return "#### ###### #####";

                    if (cardLenght <= 15)
                        return "#### #### #### ####";
                    else
                        return "#### #### #### #### ###";
            }
            return "";
        }

        private string GetPattern(int digitCount)
        {
            var pattern = string.Empty;
            for (int i = 0; i < digitCount; i++)
            {
                pattern += @"(\d)";
            }
            return pattern;
        }

        private string GetReplacement(string mask, int digitCount)
        {
            var replacement = string.Empty;
            var currentDigitIndex = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i].IsDigit() || mask[i] == '#')
                {
                    if (currentDigitIndex == digitCount)
                        break;

                    replacement += $"${++currentDigitIndex}";
                }
                else
                    replacement += mask[i];
            }
            return replacement;
        }
    }
}
