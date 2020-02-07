using System.Collections.Generic;
using System.Linq;
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

        private int _currentPlayingItemPosition = -1;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.publications_layout, null);
            InitializeControls(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            PlayFirstCompletelyVisibleVideoItem();
        }

        private void InitializeControls(View view)
        {
            _publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.publication_recycler_view);
            var dividerItemDecoration = new DividerItemDecoration(Application.Context, LinearLayoutManager.Vertical);
            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);
            _publicationRecyclerView.Adapter = new PublicationsRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);
            _stateScrollListener = new StateScrollListener();
            _publicationRecyclerView.AddOnScrollListener(_stateScrollListener);
        }

        protected override void Subscription()
        {
            _publicationTypeTabLayout.TabSelected += PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected += PublicationTypeTabLayoutTabUnselected;
            _stateScrollListener.FinishScroll += StateScrollListenerFinishScroll;
        }

        protected override void Unsubscription()
        {
            _publicationTypeTabLayout.TabSelected -= PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected -= PublicationTypeTabLayoutTabUnselected;
            _stateScrollListener.FinishScroll -= StateScrollListenerFinishScroll;
        }

        private void StateScrollListenerFinishScroll(object sender, System.EventArgs e)
        {
            PlayFirstCompletelyVisibleVideoItem();
        }

        private void PlayFirstCompletelyVisibleVideoItem()
        {
            var layoutManager = (LinearLayoutManager)_publicationRecyclerView.GetLayoutManager();
            var firstVisibleItemPosition = layoutManager.FindFirstVisibleItemPosition();
            var visibleItemsCount = layoutManager.ChildCount;
            var centralVisibleItemIndexToPlay = visibleItemsCount / 2;
            var completelyVisibleItems = new Dictionary<int, View>();
            var partiallyVisibleItems = new Dictionary<int, View>();
            var recyclerViewBounds = new Rect();
            _publicationRecyclerView.GetHitRect(recyclerViewBounds);

            for (var i = firstVisibleItemPosition; i < firstVisibleItemPosition + visibleItemsCount; i++)
            {
                var itemView = layoutManager.FindViewByPosition(i);
                var videoView = itemView.FindViewById<VideoView>(Resource.Id.video_file);
                // The IsViewPartiallyVisible method checks for completely visible state when completelyVisible option equals true.
                var videoViewBounds = new Rect();
                videoView.GetLocalVisibleRect(videoViewBounds);
                var isCompletelyVisible = videoView.IsShown
                                          && videoViewBounds.Height() == videoView.Height
                                          && videoViewBounds.Width() == videoView.Width;
                if (isCompletelyVisible)
                {
                    completelyVisibleItems.Add(i, videoView);
                }
                else
                {
                    partiallyVisibleItems.Add(i, videoView);
                }
            }

            var itemToPlay = completelyVisibleItems.FirstOrDefault();
            if (itemToPlay.Value == null)
            {
                var centralItemView = layoutManager.FindViewByPosition(centralVisibleItemIndexToPlay);
                if (centralItemView != null)
                    itemToPlay = new KeyValuePair<int, View>(centralVisibleItemIndexToPlay, centralItemView);
                else
                    return;
            }

            Debug.WriteLine("Play activated:");

            foreach (var partiallyVisibleItem in partiallyVisibleItems)
            {
                var itemViewModel = (PublicationItemViewModel)_publicationRecyclerView.Adapter.GetItem(partiallyVisibleItem.Key);
                PauseVideo(itemViewModel);
            }

            var visibleViewModel = (PublicationItemViewModel)_publicationRecyclerView.Adapter.GetItem(itemToPlay.Key);

            //if (_currentPlayingItemPosition == itemToPlay.Key && _currentPlayingItemPosition != -1)
            //    return;

            _currentPlayingItemPosition = itemToPlay.Key;

            if (itemToPlay.Value is VideoView videoItemView)
            {
                PlayVideo(visibleViewModel, videoItemView);
            }
        }

        private void PlayVideo(PublicationItemViewModel itemViewModel, VideoView videoView)
        {
            var videoService = itemViewModel.VideoPlayerService;
            videoService.Player.SetPlatformVideoPlayerContainer(videoView);
            videoService.Play(itemViewModel.VideoUrl);
        }

        private void PauseVideo(PublicationItemViewModel itemViewModel)
        {
            itemViewModel.VideoPlayerService.Stop();
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
