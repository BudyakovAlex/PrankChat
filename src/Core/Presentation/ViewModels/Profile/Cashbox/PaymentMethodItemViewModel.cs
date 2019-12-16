using System;
namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class PaymentMethodItemViewModel
    {
        public PaymentType Type { get; }

        public PaymentMethodItemViewModel(PaymentType paymentType)
        {
            Type = paymentType;
        }
    }
}
