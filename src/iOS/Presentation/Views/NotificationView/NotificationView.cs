using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
	[MvxModalPresentation(WrapInNavigationController = true)]
	public partial class NotificationView : BaseGradientBarView<NotificationViewModel>
    {
        public NotificationTableSource NotificationTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<NotificationView, NotificationViewModel>();

            set.Bind(NotificationTableSource)
                .To(vm => vm.Items);

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
            tableView.RowHeight = NotificationItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }
    }
}

