using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.Views.Base;
using PrankChat.Mobile.iOS.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class UserProfileView : BaseViewController<UserProfileViewModel>
    {
        private MvxUIRefreshControl _refreshControlProfile;
        private UIBarButtonItem _notificationBarItem;
        private OrdersTableSource _source;

        private bool _isSubscribed;
        private bool _hasDescription;

        public bool HasDescription
        {
            get => _hasDescription;
            set
            {
                _hasDescription = value;
                if (_hasDescription)
                {
                    DescriptionLabel.Hidden = false;
                    DescriptionTopConstraint.Constant = 20f;
                }
                else
                {
                    DescriptionLabel.Hidden = true;
                    DescriptionTopConstraint.Constant = 0f;
                }
            }
        }

        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                if (_isSubscribed)
                {
                    SubscribeButton.SetDarkStyle(Resources.Unsubscribe);
                    return;
                }

                SubscribeButton.SetBorderlessStyle(Resources.Subscribe, Theme.Color.Accent);
            }
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<UserProfileView, UserProfileViewModel>();

            bindingSet.Bind(this).For(v => v.HasDescription).To(vm => vm.HasDescription);
            bindingSet.Bind(_refreshControlProfile).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(this).For(v => v.IsSubscribed).To(vm => vm.IsSubscribed);
            bindingSet.Bind(_refreshControlProfile).For(v => v.RefreshCommand).To(vm => vm.RefreshUserDataCommand);
            bindingSet.Bind(_source).To(vm => vm.Items);
            bindingSet.Bind(_source).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(ProfileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(ProfileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortLogin)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(NameLabel).To(vm => vm.Login)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(DescriptionLabel).To(vm => vm.Description)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(SubscribeButton).For(v => v.BindTouchUpInside()).To(vm => vm.SubscribeCommand);
            bindingSet.Bind(SubscribersValueLabel).To(vm => vm.SubscribersValue);
            bindingSet.Bind(SubscribersView).For(v => v.BindTap()).To(vm => vm.ShowSubscribersCommand);
            bindingSet.Bind(SubscriptionsValueLabel).To(vm => vm.SubscriptionsValue);
            bindingSet.Bind(SubscriptionsView).For(v => v.BindTap()).To(vm => vm.ShowSubscriptionsCommand);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
                      .WithConversion<BoolToNotificationImageConverter>();
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            InitializeTableView();

            NameLabel.SetTitleStyle();
            SubscribersValueLabel.SetBoldTitleStyle();
            SubscribersTitleLabel.SetSmallSubtitleStyle(Resources.Subscribers);
            SubscriptionsValueLabel.SetBoldTitleStyle();
            SubscriptionsTitleLabel.SetSmallSubtitleStyle(Resources.Subscriptions);
            DescriptionLabel.SetTitleStyle();

            TabView.AddTab(
                Resources.Ordered,
                () => ViewModel.SelectedOrderType = ProfileOrderType.MyOrdered);

            TabView.AddTab(
                Resources.Execute,
                () => ViewModel.SelectedOrderType = ProfileOrderType.OrdersCompletedByMe);

            _refreshControlProfile = new MvxUIRefreshControl();
            RootScrollView.RefreshControl = _refreshControlProfile;
        }

        private void InitializeTableView()
        {
            _source = new OrdersTableSource(TableView);
            TableView.Source = _source;
            TableView.SeparatorColor = Theme.Color.Separator;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
            TableView.ContentInset = new UIEdgeInsets(10, 0, 0, 0);
        }
    }
}
