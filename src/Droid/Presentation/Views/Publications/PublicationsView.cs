using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using static Android.Support.Design.Widget.TabLayout;
using Debug = System.Diagnostics.Debug;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(PublicationsView))]
    public class PublicationsView : BaseTabFragment<PublicationsViewModel>
    {
        private TabLayout _publicationTypeTabLayout;
        private Typeface _unselectedTypeface;
        private MvxRecyclerView _publicationRecyclerView;
        private StateScrollListener _stateScrollListener;
        private PublicationsLinearLayoutManager _layoutManager;
        private int _currentPlayingItemPosition = -1;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.publications_layout, null);
            InitializeControls(view);
            return view;
        }

        protected override void Subscription()
        {
            _publicationTypeTabLayout.TabSelected += PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected += PublicationTypeTabLayoutTabUnselected;
            _stateScrollListener.FinishScroll += StateScrollListenerFinishScroll;
            _layoutManager.LayoutCompleted += LayoutManagerOnLayoutCompleted;
        }

        protected override void Unsubscription()
        {
            _publicationTypeTabLayout.TabSelected -= PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected -= PublicationTypeTabLayoutTabUnselected;
            _stateScrollListener.FinishScroll -= StateScrollListenerFinishScroll;
            _layoutManager.LayoutCompleted -= LayoutManagerOnLayoutCompleted;
        }

        private void InitializeControls(View view)
        {
            _publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.publication_recycler_view);
            var dividerItemDecoration = new DividerItemDecoration(Application.Context, LinearLayoutManager.Vertical);
            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);
            _publicationRecyclerView.Adapter = new PublicationsRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);
            _layoutManager = new PublicationsLinearLayoutManager(Context);
            _publicationRecyclerView.SetLayoutManager(_layoutManager);
            _stateScrollListener = new StateScrollListener();
            _publicationRecyclerView.AddOnScrollListener(_stateScrollListener);
        }

        private void StateScrollListenerFinishScroll(object sender, System.EventArgs e)
        {
            PlayFirstCompletelyVisibleVideoItem();
        }

        private void LayoutManagerOnLayoutCompleted(object sender, EventArgs e)
        {
            PlayFirstCompletelyVisibleVideoItem();
        }

        private void PlayFirstCompletelyVisibleVideoItem()
        {
            var visibleItemsCount = _layoutManager.ChildCount;
            var firstVisibleItemPosition = _layoutManager.FindFirstVisibleItemPosition();
            var completelyVisibleItems = new Dictionary<int, VideoView>();
            var partiallyVisibleItems = new Dictionary<int, VideoView>();

            for (var i = firstVisibleItemPosition; i < firstVisibleItemPosition + visibleItemsCount; i++)
            {
                var videoView = GetVideoViewForItemByPosition(_layoutManager, i);
                var isCompletelyVisible = IsCompletelyVisible(videoView);

                if (isCompletelyVisible)
                    completelyVisibleItems.Add(i, videoView);
                else
                    partiallyVisibleItems.Add(i, videoView);
            }

            var itemToPlay = completelyVisibleItems.FirstOrDefault();
            if (itemToPlay.Value == null)
            {
                itemToPlay = GetCentralVideoItem(_layoutManager);
            }

            if (itemToPlay.Value == null)
                itemToPlay = GetCentralVideoItem(_layoutManager);

            if (_currentPlayingItemPosition == itemToPlay.Key && _currentPlayingItemPosition != -1)
                return;

            PauseAllVideoItems(partiallyVisibleItems);

            var visibleViewModel = (PublicationItemViewModel)_publicationRecyclerView.Adapter.GetItem(itemToPlay.Key);

            if (visibleViewModel == null || itemToPlay.Value == null)
                return;

            PlayVideo(visibleViewModel, itemToPlay.Value);
            _currentPlayingItemPosition = itemToPlay.Key;
        }

        private void PlayVideo(PublicationItemViewModel itemViewModel, VideoView videoView)
        {
            Debug.WriteLine("PlayVideo [Start]");
            var videoService = itemViewModel.VideoPlayerService;
            videoService.Player.SetPlatformVideoPlayerContainer(videoView);
            videoService.Play(itemViewModel.VideoUrl, itemViewModel.VideoId);
            Debug.WriteLine("PlayVideo [End]");
        }

        private void PauseVideo(PublicationItemViewModel itemViewModel)
        {
            Debug.WriteLine("PauseVideo [Start]");
            itemViewModel.VideoPlayerService.Pause();
        }

        private VideoView GetVideoViewForItemByPosition(LinearLayoutManager layoutManager, int index)
        {
            var itemView = layoutManager.FindViewByPosition(index);
            return itemView.FindViewById<VideoView>(Resource.Id.video_file);
        }

        private KeyValuePair<int, VideoView> GetCentralVideoItem(LinearLayoutManager layoutManager)
        {
            var visibleItemsCount = layoutManager.ChildCount;
            var centralChild = layoutManager.GetChildAt(visibleItemsCount / 2);
            if (centralChild == null)
                return new KeyValuePair<int, VideoView>(0, null);

            var centralVisibleItemIndexToPlay = layoutManager.GetPosition(centralChild);
            var centralItemView = layoutManager.FindViewByPosition(centralVisibleItemIndexToPlay);
            if (centralItemView == null)
                return new KeyValuePair<int, VideoView>(centralVisibleItemIndexToPlay, null);

            var centralVideoView = centralItemView.FindViewById<VideoView>(Resource.Id.video_file);
            return new KeyValuePair<int, VideoView>(centralVisibleItemIndexToPlay, centralVideoView);
        }

        private bool IsCompletelyVisible(VideoView videoView)
        {
            var videoViewBounds = new Rect();
            videoView.GetLocalVisibleRect(videoViewBounds);
            return videoView.IsShown
                    && videoViewBounds.Height() == videoView.Height
                    && videoViewBounds.Width() == videoView.Width;
        }

        private void PauseAllVideoItems(Dictionary<int, VideoView> itemsToPause)
        {
            foreach (var partiallyVisibleItem in itemsToPause)
            {
                var partiallyVisibleItemViewModel = (PublicationItemViewModel)_publicationRecyclerView.Adapter.GetItem(partiallyVisibleItem.Key);
                PauseVideo(partiallyVisibleItemViewModel);
            }
        }

        private void PublicationTypeTabLayoutTabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
        {
            SetTypefaceStyle(e.Tab, TypefaceStyle.Normal);
        }

        private void PublicationTypeTabLayoutTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            SetTypefaceStyle(e.Tab, TypefaceStyle.Bold);

            var publicationType = (PublicationType)e.Tab.Position;
            ViewModel.SelectedPublicationType = publicationType;
        }

        private void SetTypefaceStyle(Tab tab, TypefaceStyle typefaceStyle)
        {
            var tabLayout = (LinearLayout)((ViewGroup)_publicationTypeTabLayout.GetChildAt(0)).GetChildAt(tab.Position);
            var tabTextView = (TextView)tabLayout.GetChildAt(1);

            if (_unselectedTypeface == null)
                _unselectedTypeface = tabTextView.Typeface;

            tabTextView.SetTypeface(_unselectedTypeface, typefaceStyle);
        }
    }
}
