using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxTabPresentation(TabName = "Profile", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class ProfileView : BaseGradientBarView<ProfileViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<ProfileView, ProfileViewModel>();
			set.Apply();
		}

		protected override void SetupControls()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
		}
	}
}

