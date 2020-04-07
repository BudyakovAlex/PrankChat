using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using MvvmCross.Binding;
using UIKit;
using PrankChat.Mobile.iOS.Infrastructure;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class NotificationView : BaseGradientBarView<NotificationViewModel>
    {
        private MvxUIRefreshControl _refreshControl;

        public NotificationTableSource NotificationTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<NotificationView, NotificationViewModel>();

            set.Bind(NotificationTableSource)
                .To(vm => vm.Items);

            set.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy)
                .Mode(MvxBindingMode.TwoWay);

            set.Bind(_refreshControl)
                .For(v => v.RefreshCommand)
                .To(vm => vm.UpdateNotificationsCommand);

            set.Apply();
		}

		protected override void SetupControls()
		{
            Title = Resources.NotificationView_Title;
            InitializeTableView();
        }

        private void InitializeTableView()
        {
            NotificationTableSource = new NotificationTableSource(tableView);
            tableView.Source = NotificationTableSource;
            tableView.RegisterNibForCellReuse(NotificationItemCell.Nib, NotificationItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = Constants.CellHeights.NotificationItemCellHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }
    }
}

