using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

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
