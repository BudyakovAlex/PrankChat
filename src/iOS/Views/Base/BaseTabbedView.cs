using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseTabbedView<TViewModel> : BaseGradientBarView<TViewModel>
        where TViewModel : BasePageViewModel
    {
        public BaseTabbedView()
        {
            IsTabbedView = true;
        }
    }
}
