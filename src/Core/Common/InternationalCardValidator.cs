using CreditCardValidator;
using PrankChat.Mobile.Core.Extensions;
using System.Text.RegularExpressions;

namespace PrankChat.Mobile.Core.Common
{
    public class InternationalCardValidator
    {
        private InternationalCardValidator()
        {
        }

        public static InternationalCardValidator Instance { get; } = new InternationalCardValidator();

        public bool IsValidCreditCard(string cardNumber)
        {
            return cardNumber.CreditCardBrandIgnoreLength() != CardIssuer.Unknown;
        }

        public string VisualCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return string.Empty;
            }

            cardNumber = cardNumber.WithoutSpace();
            if (cardNumber.Length <= 4)
            {
                return cardNumber;
            }

            var cardType = cardNumber.CreditCardBrandIgnoreLength();
            var mask = GetCardMask(cardType, cardNumber.Length);

            var pattern = GetPattern(cardNumber.DigitCount());
            var replacement = GetReplacement(mask, cardNumber.DigitCount());
            var formattedNumber = Regex.Replace(cardNumber.StripAllNonNumericChars(), pattern, replacement);
            return formattedNumber;
        }

        private string GetCardMask(CardIssuer cardType, int cardLenght) => cardType switch
        {
            CardIssuer.Unknown => "#### #### #### ####",
            CardIssuer.Visa => "#### #### #### ####",
            CardIssuer.Switch => "#### #### #### ####",
            CardIssuer.RuPay => "#### #### #### ####",
            CardIssuer.MasterCard => "#### #### #### ####",
            CardIssuer.Laser => "#### #### #### ####",
            CardIssuer.JCB => "#### #### #### ####",
            CardIssuer.Hipercard => "#### #### #### ####",
            CardIssuer.Discover => "#### #### #### ####",
            CardIssuer.Dankort => "#### #### #### ####",
            CardIssuer.AmericanExpress => "#### #### #### ####",
            CardIssuer.ChinaUnionPay when cardLenght <= 16 => "#### #### #### ####",
            CardIssuer.ChinaUnionPay => "###### #############",
            CardIssuer.DinersClub when cardLenght <= 14 => "#### ###### ####",
            CardIssuer.DinersClub => "#### #### #### ####",
            CardIssuer.Maestro when cardLenght <= 13 => "#### #### #####",
            CardIssuer.Maestro when cardLenght <= 15 => "#### ###### #####",
            CardIssuer.Maestro => "#### #### #### #### ###",
            _ => "",
        };

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
                    {
                        break;
                    }

                    replacement += $"${++currentDigitIndex}";
                }
                else
                {
                    replacement += mask[i];
                }
            }

            return replacement;
        }
    }
}
