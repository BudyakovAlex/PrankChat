using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class OrderDetailsNavigationParameter
    {
        public int OrderId { get; }

        public OrderDetailsNavigationParameter(int orderId)
        {
            OrderId = orderId;
        }
    }
}
