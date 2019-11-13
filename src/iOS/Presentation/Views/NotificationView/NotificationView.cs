using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class NotificationView : BaseView<NotificationViewModel>
    {
    }
}

