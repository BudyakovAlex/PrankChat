using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MediaManager;
using MediaManager.Library;
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
using VideoView = MediaManager.Platforms.Android.Video.VideoView;

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

        private int _currentVisibleItemPosition;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.publications_layout, null);
            InitializeControls(view);
            return view;
        }

        private void InitializeControls(View view)
        {
            _publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.publication_recycler_view);
            var dividerItemDecoration = new DividerItemDecoration(Application.Context, LinearLayoutManager.Vertical);
            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);
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
            var layoutManager = (LinearLayoutManager)_publicationRecyclerView.GetLayoutManager();
            var completelyVisibleItemPosition = layoutManager.FindFirstCompletelyVisibleItemPosition();
            if (completelyVisibleItemPosition == -1 || _currentVisibleItemPosition == completelyVisibleItemPosition)
                return;

            _currentVisibleItemPosition = completelyVisibleItemPosition;
            var visibleView = layoutManager.FindViewByPosition(_currentVisibleItemPosition);
            var videoView = visibleView.FindViewById<VideoView>(Resource.Id.video_file);
            var visibleViewModel = (PublicationItemViewModel)_publicationRecyclerView.Adapter.GetItem(_currentVisibleItemPosition);

            visibleViewModel.PlayVideoCommand.Execute(videoView);
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
