using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.ViewModels.Search.Items;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Publications;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Search;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchView : BaseView<SearchViewModel>, TabLayout.IOnTabSelectedListener
    {
        private FrameLayout _loadingOverlay;
        private ClearEditText _searchTextView;

        private EndlessRecyclerView _recyclerView;
        private SafeLinearLayoutManager _layoutManager;

        private RecycleViewBindableAdapter _adapter;
        private View _emptyView;
        private TextView _emptyViewTitleTextView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_search);
        }

        protected override void SetViewProperties()
        {
            InitializeEmptyView();
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _searchTextView = FindViewById<ClearEditText>(Resource.Id.search_text_view);
            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.search_recycler_view);

            _layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;

            var scrollListener = new VideoRecyclerViewScrollListener(_layoutManager);
            _recyclerView.AddOnScrollListener(scrollListener);

            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<ProfileSearchItemViewModel, ProfileSearchItemViewHolder>(Resource.Layout.cell_search)
                .AddElement<PublicationItemViewModel, PublicationItemViewHolder>(Resource.Layout.cell_publication)
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<SearchView, SearchViewModel>();

            bindingSet.Bind(_searchTextView).To(vm => vm.SearchValue);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_loadingOverlay).For(v => v.Visibility).To(vm => vm.IsBusy).WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_emptyView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            if (ViewModel is null)
            {
                return;
            }

            ViewModel.SearchTabType = tab.Position switch
            {
                1 => SearchTabType.Videos,
                2 => SearchTabType.Orders,
                _ => SearchTabType.Users
            };

            _emptyViewTitleTextView.Text = tab.Position switch
            {
                1 => Core.Localization.Resources.PublicationsNotFound,
                2 => Core.Localization.Resources.OrdersNotFound,
                _ => Core.Localization.Resources.PeoplesNotFound
            };
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        private void InitializeEmptyView()
        {
            _emptyView = FindViewById<View>(Resource.Id.empty_view);
            _emptyViewTitleTextView = _emptyView.FindViewById<TextView>(Resource.Id.title_text_view);
            _emptyViewTitleTextView.Text = Core.Localization.Resources.PeoplesNotFound;
        }
    }
}