using PrankChat.Mobile.Core.Data.Enums;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class CashboxTypeNavigationParameter
    {
        public CashboxTypeNavigationParameter(CashboxType cashboxType)
        {
            Type = cashboxType;
        }

        public CashboxType Type { get; }
    }
}
