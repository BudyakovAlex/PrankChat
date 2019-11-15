using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
	[MvxModalPresentation(WrapInNavigationController = true)]
	public partial class NotificationView : BaseView<NotificationViewModel>
    {
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupBinding();
		}

		private void SetupBinding()
		{
			var set = this.CreateBindingSet<NotificationView, NotificationViewModel>();
			set.Bind(backButton).To(vm => vm.GoBackCommand);
			set.Apply();
		}
	}
}

