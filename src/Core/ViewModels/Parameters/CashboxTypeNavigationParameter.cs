using PrankChat.Mobile.Core.Data.Enums;

namespace PrankChat.Mobile.Core.ViewModels.Parameters
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
