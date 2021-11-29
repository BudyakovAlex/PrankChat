﻿using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.SourcesAndDelegates.Search;
using PrankChat.Mobile.iOS.Views.Base;
using PrankChat.Mobile.iOS.Views.Order;
using PrankChat.Mobile.iOS.Views.Publication;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Search
{
    public partial class SearchView : BaseViewController<SearchViewModel>
    {
        private const int SearchBarRightPadding = 16;
        private const int BackButtonWidth = 40;

        private EmptyView _peoplesEmptyView;
        private EmptyView _videosEmptyView;
        private EmptyView _ordersEmptyView;

        public SearchTableSource OrdersTableSource { get; private set; }
        public SearchTableSource PeoplesTableSource { get; private set; }
        public VideoTableSource VideosTableSource { get; private set; }

        public UISearchBar SearchBar { get; set; }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<SearchView, SearchViewModel>();

            bindingSet.Bind(OrdersTableSource).To(vm => vm.Items);
            bindingSet.Bind(PeoplesTableSource).To(vm => vm.Items);
            bindingSet.Bind(VideosTableSource).To(vm => vm.Items);

            bindingSet.Bind(SearchBar).For(v => v.Text).To(vm => vm.SearchValue);
            bindingSet.Bind(loadingView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_peoplesEmptyView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxAndValueCombiner(),
                 vm => vm.IsEmpty,
                 vm => vm.IsNotBusy,
                 vm => vm.IsInitialized);
            bindingSet.Bind(_videosEmptyView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxAndValueCombiner(),
                 vm => vm.IsEmpty,
                 vm => vm.IsNotBusy,
                 vm => vm.IsInitialized);
            bindingSet.Bind(_ordersEmptyView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxAndValueCombiner(),
                 vm => vm.IsEmpty,
                 vm => vm.IsNotBusy,
                 vm => vm.IsInitialized);
        }

        protected override void SetupControls()
        {
            var backButton = NavigationItemHelper.CreateBarButton(ImageNames.IconBack, ViewModel.CloseCommand, UIColor.Black);

            var navigationBarWidth = NavigationController?.NavigationBar.Frame.Width;
            var searchBarWidth = navigationBarWidth - BackButtonWidth - SearchBarRightPadding;
            SearchBar = new UISearchBar(new CoreGraphics.CGRect(0, 0, searchBarWidth.Value, 28))
            {
                Placeholder = Resources.Search
            };

            SearchBar.SetStyle();

            videosTableView.Hidden = true;
            ordersTableView.Hidden = true;

            NavigationItem.LeftBarButtonItems = new UIBarButtonItem[]
            {
                backButton,
                new UIBarButtonItem(SearchBar)
            };

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            tabView.AddTab(Resources.Peoples, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Users;
                peoplesTableView.Hidden = false;
                videosTableView.Hidden = true;
                ordersTableView.Hidden = true;
            });
            tabView.AddTab(Resources.Videos, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Videos;
                peoplesTableView.Hidden = true;
                videosTableView.Hidden = false;
                ordersTableView.Hidden = true;
            });

            tabView.AddTab(Resources.Orders, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Orders;
                peoplesTableView.Hidden = true;
                videosTableView.Hidden = true;
                ordersTableView.Hidden = false;
            });

            SetupOrdersTableView();
            SetupVideosTableView();
            SetupPeoplesTableView();
            CreateEmptyViews();
        }

        private void SetupOrdersTableView()
        {
            OrdersTableSource = new OrdersSearchTableSource(ordersTableView);
            ordersTableView.Source = OrdersTableSource;
            ordersTableView.SetStyle();
            ordersTableView.RowHeight = Constants.CellHeights.OrderItemCellHeight;

            ordersTableView.RegisterNibForCellReuse(ProfileSearchItemCell.Nib, ProfileSearchItemCell.CellId);
            ordersTableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            ordersTableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);

            ordersTableView.UserInteractionEnabled = true;
            ordersTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        private void SetupVideosTableView()
        {
            VideosTableSource = new VideoTableSource(videosTableView);
            VideosTableSource.Register<PublicationItemViewModel>(PublicationItemCell.Nib, PublicationItemCell.CellId);
            VideosTableSource.Register<OrderItemViewModel>(OrderItemCell.Nib, OrderItemCell.CellId);
            VideosTableSource.Register<ProfileSearchItemViewModel>(ProfileSearchItemCell.Nib, ProfileSearchItemCell.CellId);

            videosTableView.SetVideoListStyle(Constants.CellHeights.PublicationItemCellHeight);

            videosTableView.Source = VideosTableSource;

            videosTableView.UserInteractionEnabled = true;
            videosTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        private void SetupPeoplesTableView()
        {
            PeoplesTableSource = new SearchTableSource(peoplesTableView);
            peoplesTableView.Source = PeoplesTableSource;
            peoplesTableView.SetStyle();
            peoplesTableView.RowHeight = UITableView.AutomaticDimension;

            peoplesTableView.RegisterNibForCellReuse(ProfileSearchItemCell.Nib, ProfileSearchItemCell.CellId);
            peoplesTableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            peoplesTableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);

            peoplesTableView.UserInteractionEnabled = true;
            peoplesTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        private void CreateEmptyViews()
        {
            _peoplesEmptyView = EmptyView
                .Create(Resources.PeoplesNotFound, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(peoplesTableView);

            _videosEmptyView = EmptyView
              .Create(Resources.PublicationsNotFound, ImageNames.ImageEmptyState)
              .AttachToTableViewAsBackgroundView(videosTableView);

            _ordersEmptyView = EmptyView
              .Create(Resources.OrdersNotFound, ImageNames.ImageEmptyState)
              .AttachToTableViewAsBackgroundView(ordersTableView);
        }
    }
}