using Android.App;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using System;
using System.Threading.Tasks;
using Android.Views;
using System.Diagnostics;

namespace PrankChat.Mobile.Droid.Presentation.Views
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
        private StateScrollListener _stateScrollListener;
        private PublicationItemViewHolder _previousVideoViewHolder;
        private TextureView _previousVideoView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_search);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _searchTextView = FindViewById<ClearEditText>(Resource.Id.search_text_view);
            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.search_recycler_view);

            _layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;

            _stateScrollListener = new StateScrollListener();
            _recyclerView.AddOnScrollListener(_stateScrollListener);

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

        protected override void Subscription()
        {
            _stateScrollListener.FinishScroll += StateScrollListenerFinishScroll;
        }

        protected override void Unsubscription()
        {
            _stateScrollListener.FinishScroll -= StateScrollListenerFinishScroll;
        }

        private void StateScrollListenerFinishScroll(object sender, EventArgs e)
        {
            PlayVisibleVideoAsync().FireAndForget();
        }

        private Task PlayVisibleVideoAsync()
        {
            var firstCompletelyVisibleItemPosition = _layoutManager.FindFirstCompletelyVisibleItemPosition();
            var lastCompletelyVisibleItemPosition = _layoutManager.FindLastCompletelyVisibleItemPosition();

            var targetPosition = firstCompletelyVisibleItemPosition;
            if (firstCompletelyVisibleItemPosition == -1)
            {
                targetPosition = lastCompletelyVisibleItemPosition;
            }

            targetPosition = targetPosition == -1
                ? _layoutManager.FindFirstVisibleItemPosition()
                : targetPosition;

            var viewHolder = _recyclerView.FindViewHolderForAdapterPosition(targetPosition);
            if (viewHolder is PublicationItemViewHolder itemViewHolder)
            {
                itemViewHolder.LoadingProgressBar.Visibility = ViewStates.Visible;
                PlayVideo(itemViewHolder, itemViewHolder.TextureView);
            }

            return Task.CompletedTask;
        }

        private void PlayVideo(PublicationItemViewHolder itemViewHolder, TextureView textureView)
        {
            if (_previousVideoViewHolder?.ViewModel != null &&
                _previousVideoViewHolder.ViewModel.VideoPlayerService != null &&
                _previousVideoView != null)
            {
                StopVideo(_previousVideoViewHolder);
            }

            Debug.WriteLine("PlayVideo [Start]");

            if (itemViewHolder?.ViewModel?.VideoPlayerService is null ||
                textureView is null ||
                itemViewHolder?.ViewModel?.PreviewUrl is null)
            {
                return;
            }

            if (itemViewHolder.ViewModel.VideoPlayerService.Player.IsPlaying)
            {
                return;
            }

            var videoService = itemViewHolder.ViewModel.VideoPlayerService;

            videoService.Player.SetPlatformVideoPlayerContainer(textureView);
            videoService.Player.VideoRenderingStartedAction = itemViewHolder.OnRenderingStarted;
            videoService.Play(itemViewHolder.ViewModel.PreviewUrl, itemViewHolder.ViewModel.VideoId);
            _previousVideoViewHolder = itemViewHolder;
            _previousVideoView = textureView;

            Debug.WriteLine("PlayVideo [End]");
        }

        private void StopVideo(PublicationItemViewHolder viewHolder)
        {
            Debug.WriteLine("StopVideo [Start]");
            if (viewHolder?.ViewModel?.VideoPlayerService?.Player is null)
            {
                return;
            }

            viewHolder.ViewModel.VideoPlayerService.Player.VideoRenderingStartedAction = null;
            viewHolder.StubImageView.Visibility = ViewStates.Visible;
            viewHolder.ViewModel.VideoPlayerService.Stop();
            viewHolder.LoadingProgressBar.Visibility = ViewStates.Invisible;
        }
    }
}