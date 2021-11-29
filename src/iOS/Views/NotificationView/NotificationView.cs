using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Notification;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Views.Base;
using PrankChat.Mobile.iOS.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.NotificationView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class NotificationView : BaseViewController<NotificationViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private NotificationTableSource _notificationTableSource;
        private EmptyView _emptyView;

        protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<NotificationView, NotificationViewModel>();

            bindingSet.Bind(_notificationTableSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy)
                .Mode(MvxBindingMode.TwoWay);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(_notificationTableSource).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

		protected override void SetupControls()
		{
            Title = Resources.Notifications;
            InitializeTableView();
            CreateEmptyView();
        }

        private void InitializeTableView()
        {
            _notificationTableSource = new NotificationTableSource(tableView);
            tableView.Source = _notificationTableSource;

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

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(Resources.NotificationsListIsEmpty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(tableView);
        }
    }
}