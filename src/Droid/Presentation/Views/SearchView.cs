using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Com.Airbnb.Lottie;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Search;
using PrankChat.Mobile.Droid.Presentation.Converters;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchView : BaseView<SearchViewModel>, TabLayout.IOnTabSelectedListener
    {
        private FrameLayout _loadingOverlay;
        private ClearEditText _searchTextView;
        private EndlessRecyclerView _recyclerView;
        private RecycleViewBindableAdapter _adapter;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_search);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _searchTextView = FindViewById<ClearEditText>(Resource.Id.search_text_view);
            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.search_recycler_view);

            var layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<ProfileSearchItemViewModel, ProfileSearchItemViewHolder>(Resource.Layout.cell_search)
                .AddElement<PublicationItemViewModel, PublicationItemViewHolder>(Resource.Layout.cell_publication)
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<SearchView, SearchViewModel>();

            bindingSet.Bind(_searchTextView)
                      .To(vm => vm.SearchValue);

            bindingSet.Bind(_recyclerView)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_loadingOverlay)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsBusy)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Apply();
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

            switch (tab.Position)
            {
                case 0:
                    ViewModel.SearchTabType = SearchTabType.Users;
                    break;
                case 1:
                    ViewModel.SearchTabType = SearchTabType.Videos;
                    break;
                case 2:
                    ViewModel.SearchTabType = SearchTabType.Orders;
                    break;
            }
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }
    }
}