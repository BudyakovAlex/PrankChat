using System;
using System.Text.RegularExpressions;
using CreditCardValidator;

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

            cardNumber = cardNumber.Replace(" ", string.Empty);
            if (cardNumber.Length <= 4)
                return cardNumber;

            var cardType = cardNumber.CreditCardBrandIgnoreLength();
            var pattern = GetCardMask(cardType);
            //var formattedNumber = Regex.Replace(cardNumber, pattern, replacement);
            return cardNumber;
        }

        private string GetCardMask(CardIssuer cardType)
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
                    // #### #### #### #### (4-4-4-4)
                    break;

                case CardIssuer.ChinaUnionPay:
                    //#### #### #### #### (4-4-4-4)
                    //###### ############# (6-13)
                    break;

                case CardIssuer.DinersClub:
                    //#### ###### #### (4-6-4)
                    //#### #### #### #### (4-4-4-4)
                    break;

                case CardIssuer.Maestro:
                    //#### #### ##### (4-4-5)
                    //#### ###### ##### (4-6-5)
                    //#### #### #### #### (4-4-4-4)
                    //#### #### #### #### ### (4-4-4-4-3)
                    break;
            }

            return "";
        }
    }
}
