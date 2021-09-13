using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Order.Sections.Abstract
{
    public abstract class BaseOrderDetailsSectionViewModel : BaseViewModel
    {
        public void SetOrder(Models.Data.Order order)
        {
            Order = order;
            RaiseAllPropertiesChanged();
        }

        public Models.Data.Order Order { get; private set; }
    }
}