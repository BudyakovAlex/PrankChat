using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = "unselected", TabSelectedIconName = "selected")]
    public partial class PublicationsView : BaseTabbedView<PublicationsViewModel>
    {
        public PublicationTableSource PublicationTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

			set.Bind(publicationTypeSegment)
				.For(v => v.SelectedSegment)
				.To(vm => vm.SelectedPublicationType)
				.WithConversion<PublicationTypeConverter>();

            set.Bind(filterContainerView.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenFilterCommand);

            set.Bind(filterTitleLabel)
                .To(vm => vm.ActiveFilterName);

            set.Bind(PublicationTableSource)
                .To(vm => vm.Items);

            set.Bind(PublicationTableSource)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectItemCommand);

            set.Apply();
		}

		protected override void SetupControls()
		{
			InitializeNavigationBar();
            InitializeTableView();

            publicationTypeSegment.SetStyle(new string[] {
				Resources.Popular_Publication_Tab,
				Resources.Actual_Publication_Tab,
				Resources.MyFeed_Publication_Tab,
			});

            topSeparatorView.BackgroundColor = Theme.Color.Separator;
            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            // Fix to select cell
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            // Fix to select cell
        }

        private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        private void InitializeTableView()
        {
            PublicationTableSource = new PublicationTableSource(tableView);
            tableView.Source = PublicationTableSource;
            tableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = PublicationItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }
    }
}

