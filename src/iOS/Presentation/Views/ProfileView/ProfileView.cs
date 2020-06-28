using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
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
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;
        private OrdersTableSource _source;

        protected override void SetupBinding()
        {
            var bindingSet = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.LoadProfileCommand);

            bindingSet.Bind(_source)
                      .To(vm => vm.Items);

            bindingSet.Bind(_source)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

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

            bindingSet.Bind(nameLabel)
                      .To(vm => vm.Name)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(descriptionLabel)
                      .To(vm => vm.Description)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(refillButton)
                      .To(vm => vm.ShowRefillCommand);

            bindingSet.Bind(withdrawalButton)
                      .To(vm => vm.ShowWithdrawalCommand);

            bindingSet.Bind(priceLabel)
                      .To(vm => vm.Price);

            bindingSet.Bind(subscribersValueLabel)
                      .To(vm => vm.SubscribersValue);

            bindingSet.Bind(subscribersView)
                      .For(v => v.BindTap())
                      .To(vm => vm.ShowSubscribersCommand);

            bindingSet.Bind(subscriptionsValueLabel)
                      .To(vm => vm.SubscriptionsValue);

            bindingSet.Bind(subscriptionsView)
                      .For(v => v.BindTap())
                      .To(vm => vm.ShowSubscriptionsCommand);

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
            NavigationItem?.SetRightBarButtonItems(
                new UIBarButtonItem[]
                {
                    _notificationBarItem,
                    NavigationItemHelper.CreateBarButton("ic_info", ViewModel.ShowWalkthrouthCommand)
                },
                true);

            nameLabel.SetTitleStyle();
            priceLabel.SetSmallSubtitleStyle();
            refillButton.SetDarkStyle(Resources.ProfileView_Refill);
            withdrawalButton.SetBorderlessStyle(Resources.ProfileView_Withdrawal);
            subscribersValueLabel.SetBoldTitleStyle();
            subscribersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscribers_Subtitle);
            subscriptionsValueLabel.SetBoldTitleStyle();
            subscriptionsTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscriptions_Subtitle);
            descriptionLabel.SetTitleStyle();

            tabView.AddTab(
                Resources.ProfileView_MyOrders_Tab,
                () => ViewModel.SelectedOrderType = ProfileOrderType.MyOrdered);

            tabView.AddTab(
                Resources.ProfileView_CompletedOrders_Tab,
                () => ViewModel.SelectedOrderType = ProfileOrderType.OrdersCompletedByMe);

            _refreshControl = new MvxUIRefreshControl();
            rootScrollView.RefreshControl = _refreshControl;

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        private void InitializeTableView()
        {
            _source = new OrdersTableSource(tableView);
            tableView.Source = _source;
            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
            tableView.ContentInset = new UIEdgeInsets(10, 0, 0, 0);
        }
    }
}
