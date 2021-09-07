using CreditCardValidator;
using PrankChat.Mobile.Core.Extensions;
using System.Text.RegularExpressions;

namespace PrankChat.Mobile.Core.Common
{
    public class InternationalCardValidator
    {
        private const string DefaultCardFormat = "#### #### #### ####";
        private const string ChinaUnionPayCardFormat = "###### #############";
        private const string DinersClubCardFormat = "#### ###### ####";
        private const string MaestroCardFormat = "#### #### #### #### ###";
        private const string MaestroLessThirteenCardFormat = "#### #### #####";
        private const string MaestroLessFifteenCartFormat = "#### ###### #####";

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

        private string GetCardMask(CardIssuer cardType, int cardLength) => cardType switch
        {
            CardIssuer.Unknown => DefaultCardFormat,
            CardIssuer.Visa => DefaultCardFormat,
            CardIssuer.Switch => DefaultCardFormat,
            CardIssuer.RuPay => DefaultCardFormat,
            CardIssuer.MasterCard => DefaultCardFormat,
            CardIssuer.Laser => DefaultCardFormat,
            CardIssuer.JCB => DefaultCardFormat,
            CardIssuer.Hipercard => DefaultCardFormat,
            CardIssuer.Discover => DefaultCardFormat,
            CardIssuer.Dankort => DefaultCardFormat,
            CardIssuer.AmericanExpress => DefaultCardFormat,
            CardIssuer.ChinaUnionPay when cardLength <= 16 => DefaultCardFormat,
            CardIssuer.ChinaUnionPay => ChinaUnionPayCardFormat,
            CardIssuer.DinersClub when cardLength <= 14 => DinersClubCardFormat,
            CardIssuer.DinersClub => DefaultCardFormat,
            CardIssuer.Maestro when cardLength <= 13 => MaestroLessThirteenCardFormat,
            CardIssuer.Maestro when cardLength <= 15 => MaestroLessFifteenCartFormat,
            CardIssuer.Maestro => MaestroCardFormat,
            _ => string.Empty,
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
