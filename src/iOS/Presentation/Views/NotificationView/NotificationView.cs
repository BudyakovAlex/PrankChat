using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class NotificationView : BaseGradientBarView<NotificationViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private NotificationTableSource _notificationTableSource;

        protected override void SetupBinding()
		{
			var bindingSet = this.CreateBindingSet<NotificationView, NotificationViewModel>();

            bindingSet.Bind(_notificationTableSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy)
                      .Mode(MvxBindingMode.TwoWay);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.ReloadItemsCommand);

            bindingSet.Bind(_notificationTableSource)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
		}

		protected override void SetupControls()
		{
            Title = Resources.NotificationView_Title;
            InitializeTableView();
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
    }
}