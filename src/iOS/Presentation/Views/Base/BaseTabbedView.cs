using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseTabbedView<TViewModel> : BaseGradientBarView<TViewModel>
        where TViewModel : BaseViewModel
    {
        public BaseTabbedView()
        {
            IsTabbedView = true;
        }
    }
}
