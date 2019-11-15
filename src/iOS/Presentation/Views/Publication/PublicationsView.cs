using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class PublicationsView : BaseView<PublicationsViewModel>
    {
		public UIBarButtonItem NotificationBarButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupControls();
			SetupBinding();
		}

		private void SetupBinding()
		{
			var set = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();
			set.Bind(NotificationBarButton).To(vm => vm.ShowNotificationCommand);
			set.Apply();
		}

		private void SetupControls()
		{
			NotificationBarButton = new UIBarButtonItem("Notification", UIBarButtonItemStyle.Plain, null);
			NavigationItem.RightBarButtonItem = NotificationBarButton;
		}
	}
}

