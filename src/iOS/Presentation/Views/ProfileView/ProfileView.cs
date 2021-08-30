using CoreGraphics;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using PrankChat.Mobile.iOS.Providers;
using System;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxTabPresentation(TabName = "Profile", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected, WrapInNavigationController = true)]
    public partial class ProfileView : BaseRefreshableTabbedView<ProfileViewModel>, IScrollableView
    {
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;
        private OrdersTableSource _source;

        private bool _hasDescription;
        public bool HasDescription
        {
            get => _hasDescription;
            set
            {
                _hasDescription = value;
                if (_hasDescription)
                {
                    descriptionLabel.Hidden = false;
                    descriptionTopConstraint.Constant = 20f;
                }
                else
                {
                    descriptionLabel.Hidden = true;
                    descriptionTopConstraint.Constant = 0f;
                }
            }
        }

        public UIScrollView ScrollView => rootScrollView;

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            var topOffset = 10 + headerContainerView.Frame.Height;
            tableView.ContentInset = new UIEdgeInsets(topOffset, 0, 0, 0);
            tableView.SetContentOffset(new CGPoint(0, -topOffset), false);
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            bindingSet.Bind(this).For(v => v.HasDescription).To(vm => vm.HasDescription);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.LoadProfileCommand);
            bindingSet.Bind(_source).To(vm => vm.Items);
            bindingSet.Bind(_source).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(profileImageView.Tap()).For(v => v.Command).To(vm => vm.ShowUpdateProfileCommand);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(nameLabel).To(vm => vm.Login)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(descriptionLabel).To(vm => vm.Description)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(refillButton).To(vm => vm.ShowRefillCommand);
            bindingSet.Bind(withdrawalButton).To(vm => vm.ShowWithdrawalCommand);
            bindingSet.Bind(priceLabel).To(vm => vm.Price);
            bindingSet.Bind(subscribersValueLabel).To(vm => vm.SubscribersValue);
            bindingSet.Bind(subscribersView).For(v => v.BindTap()).To(vm => vm.ShowSubscribersCommand);
            bindingSet.Bind(subscriptionsValueLabel).To(vm => vm.SubscriptionsValue);
            bindingSet.Bind(subscriptionsView).For(v => v.BindTap()).To(vm => vm.ShowSubscriptionsCommand);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
                      .WithConversion<BoolToNotificationImageConverter>();
        }

        protected override void SetupControls()
        {
            DefinesPresentationContext = true;

            InitializeTableView();

            _notificationBarItem = NavigationItemHelper.CreateBarButton(ImageNames.IconNotification, ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(
                new UIBarButtonItem[]
                {
                    _notificationBarItem,
                    NavigationItemHelper.CreateBarButton(ImageNames.IconInfo, ViewModel.ShowWalkthrouthCommand)
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

            tabView.AddTab(Resources.ProfileView_MyOrders_Tab, () => TabSelected(ProfileOrderType.MyOrdered));

            tabView.AddTab(Resources.ProfileView_CompletedOrders_Tab, () => TabSelected(ProfileOrderType.OrdersCompletedByMe));

            _refreshControl = new MvxUIRefreshControl();
            rootScrollView.RefreshControl = _refreshControl;

            var logoButton = NavigationItemHelper.CreateBarButton(ImageNames.IconLogo, null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        protected override void RefreshData()
        {
            if (ViewModel is null)
            {
                return;
            }

            ViewModel.LoadProfileCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel.SafeExecutionWrapper.Wrap(() =>
                rootScrollView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void TabSelected(ProfileOrderType profileOrderType)
        {
            tableView.SetContentOffset(new CGPoint(0, 0), false);
            ViewModel.SelectedOrderType = profileOrderType;
        }

        private void InitializeTableView()
        {
            _source = new OrdersTableSource(tableView, OnTableViewScrolled);
            tableView.Source = _source;
            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }

        private void OnTableViewScrolled()
        {
            var headerScrollOffset = (tableView.ContentOffset.Y + tableView.ContentInset.Top) * 0.7f;
            var headerClearOffset = -Math.Min(Math.Max(0, headerScrollOffset), headerContainerView.Frame.Height);
            headerContainerTopConstraint.Constant = (float)headerClearOffset;
        }
    }
}