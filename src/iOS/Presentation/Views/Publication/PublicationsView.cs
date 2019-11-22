using System.Windows.Input;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
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
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

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

            topSeparatorView.BackgroundColor = Theme.Color.Separator;
            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);
		}

        private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                CreateBarButton("ic_search", ViewModel.ShowNotificationCommand)
            }, true);

            var logoButton = CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        private UIBarButtonItem CreateBarButton(string imageName, ICommand command)
        {
            var imageView = UIImage.FromBundle(imageName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            return new UIBarButtonItem(imageView, UIBarButtonItemStyle.Plain,
                (sender, e) => command?.Execute(null));
        }
    }
}

