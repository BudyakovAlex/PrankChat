using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.ViewModels.Order;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Arbitration;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders;
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
        private LinearLayout _filterViewLayout;
        private TextView _filterViewTextView;
        private MvxSwipeRefreshLayout _publicationRefreshLayout;

        public OrdersView() : base(Resource.Layout.fragment_orders)
        {
        }

        public RecyclerView RecyclerView => _endlessRecyclerView;

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

        protected override void RefreshData() =>
            ViewModel?.ReloadItemsCommand.Execute();

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _endlessRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.publication_recycler_view);

            _layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _endlessRecyclerView.Adapter = _adapter;
            _filterViewLayout = view.FindViewById<LinearLayout>(Resource.Id.filter_view);
            _filterViewTextView = view.FindViewById<TextView>(Resource.Id.filter_button_title);
            _publicationRefreshLayout = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.publication_refresh_layout);

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order)
                .AddElement<ArbitrationOrderItemViewModel, ArbitrationItemViewHolder>(Resource.Layout.cell_arbitration_order);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<OrdersView, OrdersViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_endlessRecyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_filterViewLayout).For(v => v.BindClick()).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(_filterViewTextView).For(v => v.Text).To(vm => vm.ActiveFilterName);
            bindingSet.Bind(_publicationRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_publicationRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
        }
    }
}
