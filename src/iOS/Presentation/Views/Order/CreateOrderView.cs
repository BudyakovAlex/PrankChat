using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Create Order", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class CreateOrderView : BaseTabbedView<CreateOrderViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<CreateOrderView, CreateOrderViewModel>();
			set.Apply();
		}

		protected override void SetupControls()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
		}
	}
}

