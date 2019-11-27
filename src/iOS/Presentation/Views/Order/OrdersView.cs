using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Orders", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class OrdersView : BaseGradientBarView<OrdersViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<OrdersView, OrdersViewModel>();
			set.Apply();
		}

		protected override void SetupControls()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
		}
	}
}