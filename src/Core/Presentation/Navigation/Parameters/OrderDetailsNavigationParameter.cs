using System;
namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class OrderDetailsNavigationParameter : INavigationParameter
    {
        public string Id { get; }

        public OrderDetailsNavigationParameter(string id)
        {
            Id = id;
        }
    }
}
