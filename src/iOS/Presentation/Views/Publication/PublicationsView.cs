using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = "unselected", TabSelectedIconName = "selected")]
    public partial class PublicationsView : BaseView<PublicationsViewModel>
    {
		public UIBarButtonItem NotificationBarButton { get; set; }

		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();
			set.Bind(NotificationBarButton)
				.To(vm => vm.ShowNotificationCommand);

			set.Bind(publicationTypeSegment)
				.For(v => v. SelectedSegment)
				.To(vm => vm.SelectedPublicationType)
				.WithConversion<PublicationTypeConverter>();
			set.Apply();
		}

		protected override void SetupControls()
		{
			InitializeNavigationBar();

			publicationTypeSegment.SetPublicationSegmentedControlStyle(new string[] {
				Resources.Popular_Publication_Tab,
				Resources.Actual_Publication_Tab,
				Resources.MyFeed_Publication_Tab,
			});
		}

		private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
			NotificationBarButton = new UIBarButtonItem("Notification", UIBarButtonItemStyle.Plain, null);
			NavigationItem.RightBarButtonItem = NotificationBarButton;
		}
	}
}

