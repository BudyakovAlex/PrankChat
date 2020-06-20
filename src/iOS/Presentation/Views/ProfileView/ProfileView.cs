using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxTabPresentation(TabName = "Profile", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class ProfileView : BaseTabbedView<ProfileViewModel>
    {
        private MvxUIRefreshControl _refreshControlProfile;
        private UIBarButtonItem _notificationBarItem;

        public OrdersTableSource OrdersTableSource { get; private set; }

        protected override void SetupBinding()
        {
            var bindingSet = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            bindingSet.Bind(_refreshControlProfile)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshControlProfile)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.LoadProfileCommand);

            bindingSet.Bind(OrdersTableSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(OrdersTableSource)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);
                    
            bindingSet.Bind(profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(profileImageView.Tap())
                      .For(v => v.Command)
                      .To(vm => vm.ShowUpdateProfileCommand);

            bindingSet.Bind(profileImageView)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortName)
                      .Mode(MvxBindingMode.OneTime);

            bindingSet.Bind(profileDescriptionLabel)
                      .To(vm => vm.Name)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(refillButton)
                      .To(vm => vm.ShowRefillCommand);

            bindingSet.Bind(withdrawalButton)
                      .To(vm => vm.ShowWithdrawalCommand);

            bindingSet.Bind(priceLabel)
                      .To(vm => vm.Price);

            bindingSet.Bind(ordersValueLabel)
                      .To(vm => vm.OrdersValue);

            bindingSet.Bind(completedOrdersValueLabel)
                      .To(vm => vm.CompletedOrdersValue);

            bindingSet.Bind(subscribersValueLabel)
                      .To(vm => vm.SubscribersValue);

            bindingSet.Bind(subscriptionsValueLabel)
                      .To(vm => vm.SubscriptionsValue);

            bindingSet.Bind(_notificationBarItem)
                      .For(v => v.Image)
                      .To(vm => vm.HasUnreadNotifications)
                      .WithConversion<BoolToNotificationImageConverter>();

            bindingSet.Apply();
        }

        protected override void SetupControls()
        {
            DefinesPresentationContext = true;

            InitializeTableView();

            _notificationBarItem = NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                _notificationBarItem,
                NavigationItemHelper.CreateBarButton("ic_info", ViewModel.ShowWalkthrouthCommand)
            }, true);

            profileDescriptionLabel.SetTitleStyle();
            priceLabel.SetSmallSubtitleStyle();
            refillButton.SetDarkStyle(Resources.ProfileView_Refill);
            withdrawalButton.SetBorderlessStyle(Resources.ProfileView_Withdrawal);
            ordersValueLabel.SetBoldTitleStyle();
            ordersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Orders_Subtitle);
            completedOrdersValueLabel.SetBoldTitleStyle();
            completedOrdersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Completed_Subtitle);
            subscribersValueLabel.SetBoldTitleStyle();
            subscribersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscribers_Subtitle);
            subscriptionsValueLabel.SetBoldTitleStyle();
            subscriptionsTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscriptions_Subtitle);

            MyOrdersTabLabel.SetTitleStyle(Resources.ProfileView_MyOrders_Tab);
            ExecutedOrdersTabLabel.SetTitleStyle(Resources.ProfileView_CompletedOrders_Tab);

            MyOrdersTabView.AddGestureRecognizer(new UITapGestureRecognizer(() => SetSelectedTab(0)));
            ExecutedOrdersTabView.AddGestureRecognizer(new UITapGestureRecognizer(() => SetSelectedTab(1)));

            _refreshControlProfile = new MvxUIRefreshControl();
            rootScrollView.RefreshControl = _refreshControlProfile;
            ApplySelectedTabStyle(0);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        public void SetSelectedTab(int index)
        {
            ApplySelectedTabStyle(index);
            switch (index)
            {
                case 0:
                    ViewModel.SelectedOrderType = ProfileOrderType.MyOrdered;
                    break;

                case 1:
                    ViewModel.SelectedOrderType = ProfileOrderType.OrdersCompletedByMe;
                    break;
            }
        }

        private void ApplySelectedTabStyle(int index)
        {
            var isFirstTabSelected = index == 0;

            MyOrdersTabIndicatorView.Hidden = !isFirstTabSelected;
            ExecutedOrdersTabIndicatorView.Hidden = isFirstTabSelected;

            if (isFirstTabSelected)
            {
                MyOrdersTabLabel.SetMainTitleStyle();
                ExecutedOrdersTabLabel.SetTitleStyle();
                return;
            }

            ExecutedOrdersTabLabel.SetMainTitleStyle();
            MyOrdersTabLabel.SetTitleStyle();
        }

        private void InitializeTableView()
        {
            OrdersTableSource = new OrdersTableSource(tableView);
            tableView.Source = OrdersTableSource;
            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
            tableView.ContentInset = new UIEdgeInsets(10, 0, 0, 0);
        }
    }
}
