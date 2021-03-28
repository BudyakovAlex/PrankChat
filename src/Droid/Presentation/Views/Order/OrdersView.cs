﻿using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Arbitration;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(OrdersView))]
    public class OrdersView : BaseRefreshableTabFragment<OrdersViewModel>, TabLayout.IOnTabSelectedListener, IScrollableView
    {
        private EndlessRecyclerView _endlessRecyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;
        private StateScrollListener _stateScrollListener;

        public RecyclerView RecyclerView => _endlessRecyclerView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_orders, null);

            InitializeControls(view);
            DoBind();
           
            return view;
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            RecyclerView.Post(() => RecyclerView.ScrollToPosition(0));
            ViewModel.TabType = (OrdersTabType)tab.Position;
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        protected override void RefreshData()
        {
            ViewModel?.ReloadItemsCommand.Execute();
        }

        private void InitializeControls(View view)
        {
            _endlessRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.publication_recycler_view);

            _layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _endlessRecyclerView.Adapter = _adapter;

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order)
                .AddElement<ArbitrationOrderItemViewModel, ArbitrationItemViewHolder>(Resource.Layout.cell_arbitration_order);

            _stateScrollListener = new StateScrollListener();
            _endlessRecyclerView.AddOnScrollListener(_stateScrollListener);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        private void DoBind()
        {
            var bindingSet = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_endlessRecyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
        }
    }
}
