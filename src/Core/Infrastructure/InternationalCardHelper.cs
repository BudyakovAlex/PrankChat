using System.Collections.Generic;
using System.Text.RegularExpressions;
using CreditCardValidator;

namespace PrankChat.Mobile.Core.Infrastructure
{
    public class InternationalCardHelper
    {
        private static readonly InternationalCardHelper _instance = new InternationalCardHelper();
        public static InternationalCardHelper Instance = _instance;

        private static IReadOnlyDictionary<CardPatternType, string> GroupSizeToPatternMap = new Dictionary<CardPatternType, string>
        {
            { CardPatternType.Default16, GenerateCardPattern(4, 4, 4, 4) },
            { CardPatternType.ChinaUnionPay19, GenerateCardPattern(6, 13) },
            { CardPatternType.DinersClub14, GenerateCardPattern(4, 6, 4) },
            { CardPatternType.Maestro13, GenerateCardPattern(4, 4, 5) },
            { CardPatternType.Maestro15, GenerateCardPattern(4, 4, 5) },
            { CardPatternType.Maestro19, GenerateCardPattern(4, 4, 4, 4, 3) }
        };

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
            var pattern = GetVerifiedPattern(cardType, cardNumber.Length);
            var match = Regex.Match(cardNumber, pattern);
            var replacement = GetReplacementPattern(match.Groups.Count);
            var formattedNumber = Regex.Replace(cardNumber, pattern, replacement);
            return formattedNumber;
        }

        private string GetReplacementPattern(int groupCount)
        {
            var pattern = string.Empty;
            var finalCount = groupCount - 1;
            for (var i = 1; i < finalCount; i++)
            {
                pattern += $"${i} ";
            }

            pattern += $"${finalCount}";

            return pattern;
        }

        private string GetVerifiedPattern(CardIssuer cardIssuer, int numberLenght)
        {
            if (cardIssuer == CardIssuer.Maestro)
            {
                if (numberLenght <= 13)
                    return GroupSizeToPatternMap[CardPatternType.Maestro13];

                if (numberLenght <= 15)
                    return GroupSizeToPatternMap[CardPatternType.Maestro15];

                if (numberLenght <= 19)
                    return GroupSizeToPatternMap[CardPatternType.Maestro19];
            }

            if(cardIssuer == CardIssuer.DinersClub && numberLenght <= 14)
                return GroupSizeToPatternMap[CardPatternType.DinersClub14];

            if (numberLenght <= 16)
                return GroupSizeToPatternMap[CardPatternType.Default16];

            if (cardIssuer == CardIssuer.ChinaUnionPay && numberLenght <= 19)
                return GroupSizeToPatternMap[CardPatternType.ChinaUnionPay19];

            return string.Empty;
        }

        private static string GenerateCardPattern(params int[] groupSizes)
        {
            var pattern = @"^";

            for (var i = 0; i < groupSizes.Length; i++)
            {
                pattern += @"([\d]{" + groupSizes[i] + @"})";
            }

            return pattern;
        }

        public enum CardPatternType
        {
            Default16,
            ChinaUnionPay19,
            DinersClub14,
            Maestro13,
            Maestro15,
            Maestro19
        }
    }
}
