﻿using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Converters;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;
using Xamarin.Essentials;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Binding.Combiners;
using PrankChat.Mobile.Core.Localization;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxTabPresentation(TabName = "Competitions", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected, WrapInNavigationController = true)]
    public partial class CompetitionsView : BaseRefreshableTabbedViewController<CompetitionsViewModel>, IScrollableView
    {
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;
        private UIBarButtonItem _notificationBarItem;
        private EmptyView _emptyView;

        public UIScrollView ScrollView => tableView;

        protected override void SetupControls()
        {
            DefinesPresentationContext = true;

            base.SetupControls();
            InitializeNavigationBar();
            CreateEmptyView();

            _source = new TableViewSource(tableView)
                .Register<CompetitionsSectionViewModel>(CompetitionsSectionCell.Nib, CompetitionsSectionCell.CellId);

            tableView.Source = _source;
            tableView.ContentInset = new UIEdgeInsets(16f, 0, 0f, 0f);

            _refreshControl = new MvxUIRefreshControl
            {
                TintColor = UIColor.White
            };

            tableView.RefreshControl = _refreshControl;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
            bindingSet.Bind(_notificationBarItem)
                .For(v => v.Image)
                .To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
                .WithConversion<BoolToNotificationImageConverter>();

            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

        protected override void RefreshData()
        {
            ViewModel?.LoadDataCommand.Execute();
            MainThread.BeginInvokeOnMainThread(() =>
                ViewModel?.SafeExecutionWrapper?.Wrap(() =>
                tableView.SetContentOffset(new CGPoint(0, -_refreshControl.Frame.Height), true)));
        }

        private void InitializeNavigationBar()
        {
            NavigationController.NavigationBar.SetNavigationBarStyle();

            _notificationBarItem = NavigationItemHelper.CreateBarButton(ImageNames.IconNotification, ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
               _notificationBarItem,
                NavigationItemHelper.CreateBarButton(ImageNames.IconInfo, ViewModel.ShowWalkthrouthCommand),
                // TODO: This feature will be implemented.
                //NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            NavigationItem.LeftBarButtonItem = NavigationItemHelper.CreateBarLogoButton();
        }

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(Resources.CompetitionsListIsEmpty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(tableView);
        }
    }
}
