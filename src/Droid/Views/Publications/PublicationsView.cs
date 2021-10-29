using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Publication;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Publications;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Views.Base;
using System;
using System.Threading.Tasks;
using static Google.Android.Material.Tabs.TabLayout;

namespace PrankChat.Mobile.Droid.Views.Publications
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.tabs,
        ViewPagerResourceId = Resource.Id.viewpager,
        ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(PublicationsView))]
    public class PublicationsView : BaseRefreshableTabFragment<PublicationsViewModel>, IScrollableView
    {
        private const int MillisecondsDelay = 300;

        private ExtendedTabLayout _publicationTypeTabLayout;
        private Typeface _unselectedTypeface;
        private EndlessRecyclerView _publicationRecyclerView;
        private FrameLayout _loadingOverlay;
        private VideoRecyclerViewScrollListener _stateScrollListener;

        private SafeLinearLayoutManager _layoutManager;
        private MvxSwipeRefreshLayout _swipeRefreshLayout;
        private TextView _filterTitleTextView;
        private View _filterView;
        private View _filterContainerView;
        private View _filterDividerView;

        private MvxInteraction _itemsChangedInteraction;

        public PublicationsView() : base(Resource.Layout.publications_layout)
        {
        }

        public MvxInteraction ItemsChangedInteraction
        {
            get => _itemsChangedInteraction;
            set
            {
                if (_itemsChangedInteraction != null)
                {
                    _itemsChangedInteraction.Requested -= OnDataSetChanged;
                }

                _itemsChangedInteraction = value;
                _itemsChangedInteraction.Requested += OnDataSetChanged;
            }
        }

        public RecyclerView RecyclerView => _publicationRecyclerView;

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _swipeRefreshLayout = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);
            _filterTitleTextView = view.FindViewById<TextView>(Resource.Id.filter_button_title);
            _filterView = view.FindViewById<View>(Resource.Id.filter_view);
            _filterContainerView = view.FindViewById<View>(Resource.Id.filter_container_view);
            _filterDividerView = view.FindViewById<View>(Resource.Id.filter_view_divider);
            _publicationTypeTabLayout = view.FindViewById<ExtendedTabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.publication_recycler_view);
            _loadingOverlay = view.FindViewById<FrameLayout>(Resource.Id.loading_overlay);

            _layoutManager = new SafeLinearLayoutManager(Context, LinearLayoutManager.Vertical, false)
            {
                InitialPrefetchItemCount = 40,
                MeasurementCacheEnabled = true,
                ItemPrefetchEnabled = true
            };

            _publicationRecyclerView.SetLayoutManager(_layoutManager);
            _publicationRecyclerView.HasFixedSize = true;
            _publicationRecyclerView.HasNextPage = true;
            _publicationRecyclerView.NestedScrollingEnabled = false;

            _publicationRecyclerView.Adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _publicationRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<PublicationItemViewModel, PublicationItemViewHolder>(Resource.Layout.cell_publication);

            _stateScrollListener = new VideoRecyclerViewScrollListener(_layoutManager);
            _publicationRecyclerView.AddOnScrollListener(_stateScrollListener);

            var dividerItemDecoration = new DividerItemDecoration(Context, LinearLayoutManager.Vertical);
            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(_swipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_swipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);

            bindingSet.Bind(_filterTitleTextView).For(v => v.Text).To(vm => vm.ActiveFilterName);
            bindingSet.Bind(_filterContainerView).For(v => v.BindClick()).To(vm => vm.OpenFilterCommand);
            bindingSet.Bind(_publicationRecyclerView).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(this).For(v => v.ItemsChangedInteraction).To(vm => vm.ItemsChangedInteraction).OneWay();
            bindingSet.Bind(_publicationRecyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_loadingOverlay).For(v => v.Visibility).To(vm => vm.IsRefreshingData)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Bind(_publicationTypeTabLayout).For(v => v.IsSelectionEnabled).To(vm => vm.IsRefreshingData)
                      .WithConversion<MvxInvertedBooleanConverter>();
        }

        protected override void RefreshData() =>
            ViewModel?.ReloadItemsCommand.Execute();

        protected override void Subscription()
        {
            _publicationTypeTabLayout.TabSelected += PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected += PublicationTypeTabLayoutTabUnselected;
        }

        protected override void Unsubscription()
        {
            _publicationTypeTabLayout.TabSelected -= PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected -= PublicationTypeTabLayoutTabUnselected;
            _itemsChangedInteraction.Requested -= OnDataSetChanged;
        }

        private void OnDataSetChanged(object sender, EventArgs e)
        {
            PlayVideoAfterReloadDataAsync().FireAndForget();
        }

        private async Task PlayVideoAfterReloadDataAsync()
        {
            await Task.Delay(MillisecondsDelay);
            await _stateScrollListener.PlayVisibleVideoAsync(RecyclerView);
        }

        private void PublicationTypeTabLayoutTabUnselected(object sender, TabUnselectedEventArgs e)
        {
            SetTypefaceStyle(e.Tab, TypefaceStyle.Normal);
        }

        private void PublicationTypeTabLayoutTabSelected(object sender, TabSelectedEventArgs e)
        {
            SetTypefaceStyle(e.Tab, TypefaceStyle.Bold);

            var publicationType = (PublicationType)e.Tab.Position;

            RecyclerView.Post(() => RecyclerView.ScrollToPosition(0));

            _filterView.Visibility = e.Tab.Position == 0
                ? ViewStates.Visible
                : ViewStates.Gone;
            _filterDividerView.Visibility = _filterView.Visibility;

            ViewModel.SelectedPublicationType = publicationType;
        }

        private void SetTypefaceStyle(Tab tab, TypefaceStyle typefaceStyle)
        {
            var tabLayout = (LinearLayout)((ViewGroup)_publicationTypeTabLayout.GetChildAt(0)).GetChildAt(tab.Position);
            var tabTextView = (TextView)tabLayout.GetChildAt(1);

            if (_unselectedTypeface == null)
            {
                _unselectedTypeface = tabTextView.Typeface;
            }

            tabTextView.SetTypeface(_unselectedTypeface, typefaceStyle);
        }
    }
}
