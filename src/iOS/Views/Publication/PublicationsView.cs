using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Extensions.MvvmCross;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Publication;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected)]
    public partial class PublicationsView : BaseRefreshableTabbedViewController<PublicationsViewModel>, IScrollableView
    {
        private UIBarButtonItem _inviteFriendBarItem;
        private UIBarButtonItem _notificationBarItem;
        private MvxUIRefreshControl _refreshControl;
        private EmptyView _emptyView;

        public VideoTableSource PublicationTableSource { get; private set; }

        public UIScrollView ScrollView => tableView;

        protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(filterContainerView.Tap()).For(v => v.Command).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(filterTitleLabel).To(vm => vm.ActiveFilterName);

            bindingSet.Bind(PublicationTableSource).To(vm => vm.Items);
            bindingSet.Bind(PublicationTableSource).For(v => v.ItemsChangedInteraction).To(vm => vm.ItemsChangedInteraction);
            bindingSet.Bind(PublicationTableSource).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(loadingOverlayView).For(v => v.BindVisible()).To(vm => vm.IsRefreshingData);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);

            bindingSet.Bind(_inviteFriendBarItem)
                .For(v => v.Image)
                .To(vm => vm.InviteFriendItemViewModel.HasBadge)
                .WithConversion((bool hasInviteFriendBadge) => CreateInviteFriendImage(hasInviteFriendBadge));

            bindingSet.Bind(_notificationBarItem)
                .For(v => v.Image)
                .To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
                .WithConversion<BoolToNotificationImageConverter>();

            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotRefreshingData,
                  vm => vm.IsInitialized);
        }

		protected override void SetupControls()
		{
			InitializeNavigationBar();
            InitializeTableView();
            CreateEmptyView();

            publicationTypeStackView.SetTabsStyle(new string[] {
                Resources.Popular,
                Resources.Actual,
				Resources.MyFeed,
			}, OnTabSelected);

            topSeparatorView.BackgroundColor = Theme.Color.Separator;
            filterArrowImageView.Image = UIImage.FromBundle(ImageNames.IconFilterArrow);
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();
        }

        protected override void RefreshData()
        {
            ViewModel?.ReloadItemsCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel.SafeExecutionWrapper.Wrap(() =>
                tableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void OnTabSelected(int position)
        {
            publicationTypeStackView.SetSelectedTabStyle(position);
            tableView.SetContentOffset(new CGPoint(0, 0), false);

            filterView.Hidden = position != 0;
            bottomSeparatorView.Hidden = filterView.Hidden;

            ViewModel.SelectedPublicationType = (PublicationType)position;
        }

        private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();

            InitializeBarButtonItems(ViewModel.InviteFriendItemViewModel.CanInviteFriend);

            var barButtonItems = GetBarButtonItems().ToArray();
            NavigationItem?.SetRightBarButtonItems(barButtonItems, true);

            NavigationItem.LeftBarButtonItem = NavigationItemHelper.CreateBarLogoButton();
        }

        private void InitializeTableView()
        {
            PublicationTableSource = new VideoTableSource(tableView);
            PublicationTableSource.Register<PublicationItemViewModel>(PublicationItemCell.Nib, PublicationItemCell.CellId);

            tableView.Source = PublicationTableSource;
            tableView.SetVideoListStyle(Constants.CellHeights.PublicationItemCellHeight);

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        private void InitializeBarButtonItems(bool canInviteFriend)
        {
            if (canInviteFriend)
            {
                _inviteFriendBarItem = NavigationItemHelper.CreateBarButton(
                    ImageNames.IconInviteFriend,
                    ViewModel.InviteFriendItemViewModel.InviteFriendCommand);
            }

            _notificationBarItem = NavigationItemHelper.CreateBarButton(
                ImageNames.IconNotification,
                ViewModel.ShowNotificationCommand);
        }

        private IEnumerable<UIBarButtonItem> GetBarButtonItems()
        {
            if (_inviteFriendBarItem != null)
            {
                yield return _inviteFriendBarItem;
            }

            yield return _notificationBarItem;
            yield return NavigationItemHelper.CreateBarButton(ImageNames.IconSearch, ViewModel.ShowSearchCommand);
        }

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(Resources.PublicationListIsEmpty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(tableView);
        }

        private UIImage CreateInviteFriendImage(bool hasInviteFriendBadge)
        {
            var imageName = hasInviteFriendBadge
                ? ImageNames.IconInviteFriendWithBadge
                : ImageNames.IconInviteFriend;

            return UIImage
                .FromBundle(imageName)
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
        }
    }
}