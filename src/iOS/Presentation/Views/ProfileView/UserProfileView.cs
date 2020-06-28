using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class UserProfileView : BaseGradientBarView<UserProfileViewModel>
    {
        private MvxUIRefreshControl _refreshControlProfile;
        private UIBarButtonItem _notificationBarItem;
        private OrdersTableSource _source;
        private bool _isSubscribed;

        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                if (_isSubscribed)
                {
                    SubscribeButton.SetDarkStyle(Resources.OrderDetailsView_Unsubscribe_Button);
                    return;
                }

                SubscribeButton.SetBorderlessStyle(Resources.OrderDetailsView_Subscribe_Button, Theme.Color.Accent);
            }
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<UserProfileView, UserProfileViewModel>();

            bindingSet.Bind(_refreshControlProfile)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(this)
                      .For(v => v.IsSubscribed)
                      .To(vm => vm.IsSubscribed);

            bindingSet.Bind(_refreshControlProfile)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshUserDataCommand);

            bindingSet.Bind(_source)
                      .To(vm => vm.Items);

            bindingSet.Bind(_source)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(ProfileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(ProfileImageView)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortLogin)
                      .Mode(MvxBindingMode.OneTime);

            bindingSet.Bind(NameLabel)
                      .To(vm => vm.Login)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(DescriptionLabel)
                      .To(vm => vm.Description)
                      .Mode(MvxBindingMode.OneWay);

            bindingSet.Bind(SubscribeButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.SubscribeCommand);

            bindingSet.Bind(SubscribersValueLabel)
                      .To(vm => vm.SubscribersValue);

            bindingSet.Bind(SubscribersView)
                      .For(v => v.BindTap())
                      .To(vm => vm.ShowSubscribersCommand);

            bindingSet.Bind(SubscriptionsValueLabel)
                      .To(vm => vm.SubscriptionsValue);

            bindingSet.Bind(SubscriptionsView)
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
            base.SetupControls();

            InitializeTableView();

            NameLabel.SetTitleStyle();
            SubscribersValueLabel.SetBoldTitleStyle();
            SubscribersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscribers_Subtitle);
            SubscriptionsValueLabel.SetBoldTitleStyle();
            SubscriptionsTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscriptions_Subtitle);
            DescriptionLabel.SetTitleStyle();

            TabView.AddTab(
                Resources.ProfileView_MyOrders_Tab,
                () => ViewModel.SelectedOrderType = ProfileOrderType.MyOrdered);

            TabView.AddTab(
                Resources.ProfileView_CompletedOrders_Tab,
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
