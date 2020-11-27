using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract
{
    public abstract class BaseOrderDetailsSectionViewModel : BaseViewModel
    {
        public void SetOrder(OrderDataModel order)
        {
            Order = order;
            RaiseAllPropertiesChanged();
        }

        public OrderDataModel Order { get; private set; }
    }
}