using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
	[MvxTabPresentation(TabName = "Raiting", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
	public partial class RatingView : BaseTabbedView<RatingViewModel>
	{
		protected override void SetupBinding()
		{
		}

		protected override void SetupControls()
		{
		}
	}
}

