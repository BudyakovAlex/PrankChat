﻿namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class CashboxTypeNavigationParameter : INavigationParameter
    {
        public enum CashboxType
        {
            Refill,
            Withdrawal
        }

        public CashboxType Type { get; }

        public CashboxTypeNavigationParameter(CashboxType cashboxType)
        {
            Type = cashboxType;
        }
    }
}
