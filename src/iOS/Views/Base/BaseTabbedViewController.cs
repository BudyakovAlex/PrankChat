using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseTabbedViewController<TViewModel> : BaseViewController<TViewModel>
        where TViewModel : BasePageViewModel
    {
        public BaseTabbedViewController()
        {
            IsTabbedView = true;
        }
    }
}
