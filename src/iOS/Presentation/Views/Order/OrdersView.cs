using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class OrdersView : BaseView<OrdersViewModel>
    {
    }
}