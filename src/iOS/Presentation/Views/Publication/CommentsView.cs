using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
	[MvxRootPresentation(WrapInNavigationController = true)]
	public partial class CommentsView : BaseView<CommentsViewModel>
	{
		protected override void SetupBinding()
		{
		}

		protected override void SetupControls()
		{
		}
	}
}

