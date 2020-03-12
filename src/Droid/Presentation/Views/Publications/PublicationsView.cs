using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications;
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
        private RecycleViewBindableAdapter _adapter;

        private PublicationItemViewModel _previousPublicationViewModel;
        public bool _isRecyclerLayoutInitialized;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.publications_layout, null);
            InitializeControls(view);
            DoBind();
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

        private void DoBind()
        {
            var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Apply();
        }

        private void InitializeControls(View view)
        {
            _publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.publication_recycler_view);
            var dividerItemDecoration = new DividerItemDecoration(Application.Context, LinearLayoutManager.Vertical);

            _layoutManager = new PublicationsLinearLayoutManager(Context);
            _publicationRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext, onViewDetachedFromWindow: OnViewHolderDetached);
            _publicationRecyclerView.Adapter = _adapter;
            _publicationRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<PublicationItemViewModel, PublicationItemViewHolder>(Resource.Layout.cell_publication);

            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);
            _stateScrollListener = new StateScrollListener();
            _publicationRecyclerView.AddOnScrollListener(_stateScrollListener);
        }

        private void OnViewHolderDetached(Java.Lang.Object holder)
        {
            if (holder is PublicationItemViewHolder viewHolder)
            {
                PauseVideo(viewHolder.ViewModel);
            }
        }

        private void StateScrollListenerFinishScroll(object sender, EventArgs e)
        {
            PlayVisibleVideoItem();
        }

        private void LayoutManagerOnLayoutCompleted(object sender, EventArgs e)
        {
            if (_isRecyclerLayoutInitialized)
            {
                return;
            }

            _isRecyclerLayoutInitialized = true;
            PlayVisibleVideoItem();
        }

        private void PlayVisibleVideoItem()
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

            var viewHolder = _publicationRecyclerView.FindViewHolderForAdapterPosition(targetPosition);
            if (viewHolder is PublicationItemViewHolder itemViewHolder)
            {
                PlayVideo(itemViewHolder.ViewModel, itemViewHolder.VideoView);
            }
        }

        private void PlayVideo(PublicationItemViewModel itemViewModel, VideoView videoView)
        {
            if (_previousPublicationViewModel != null &&
                _previousPublicationViewModel.VideoPlayerService != null)
            {
                _previousPublicationViewModel.VideoPlayerService.Stop();
            }

            Debug.WriteLine("PlayVideo [Start]");
            var videoService = itemViewModel.VideoPlayerService;
            videoService.Player.SetPlatformVideoPlayerContainer(videoView);
            videoService.Play(itemViewModel.VideoUrl, itemViewModel.VideoId);
            _previousPublicationViewModel = itemViewModel;
            Debug.WriteLine("PlayVideo [End]");
        }

        private void PauseVideo(PublicationItemViewModel itemViewModel)
        {
            Debug.WriteLine("PauseVideo [Start]");
            itemViewModel.VideoPlayerService.Pause();
        }

        private void PublicationTypeTabLayoutTabUnselected(object sender, TabUnselectedEventArgs e)
        {
            SetTypefaceStyle(e.Tab, TypefaceStyle.Normal);
        }

        private void PublicationTypeTabLayoutTabSelected(object sender, TabSelectedEventArgs e)
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