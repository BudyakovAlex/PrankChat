using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
	[MvxTabPresentation(TabName = "Raiting", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
	public partial class RatingView : BaseTabbedView<RatingViewModel>
	{
        private MvxUIRefreshControl _refreshControl;

        public RatingTableSource RatingTableSource { get; private set; }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<RatingView, RatingViewModel>();

            set.Bind(RatingTableSource)
                .To(vm => vm.Items);

            set.Bind(filterContainerView.Tap())
                .For(v => v.Command)
                .To(vm => vm.OpenFilterCommand);

            set.Bind(filterTitleLabel)
                .To(vm => vm.ActiveFilterName);

            set.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
                .For(v => v.RefreshCommand)
                .To(vm => vm.LoadRatingOrdersCommand);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = Resources.RateView_Title_Label;

            InitializeTableView();
            InitializeNavigationBar();

            NavigationController.NavigationBar.SetNavigationBarStyle();

            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        private void InitializeTableView()
        {
            RatingTableSource = new RatingTableSource(tableView);
            tableView.Source = RatingTableSource;
            tableView.RegisterNibForCellReuse(RatingItemCell.Nib, RatingItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = RatingItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }

        private void InitializeNavigationBar()
        {
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);
        }
    }
}

