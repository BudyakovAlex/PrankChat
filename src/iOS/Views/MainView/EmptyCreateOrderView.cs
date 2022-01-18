using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.MainView
{
    [MvxTabPresentation(TabName = "Create Order", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected, WrapInNavigationController = true)]
    public partial class EmptyCreateOrderView : BaseTabbedViewController<EmptyCreateOrderViewModel>
    {
    }
}

