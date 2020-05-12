using System;
using System.Threading.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters;
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
        private const int MillisecondsDelay = 300;

        private TabLayout _publicationTypeTabLayout;
        private Typeface _unselectedTypeface;

        private EndlessRecyclerView _publicationRecyclerView;

        private StateScrollListener _stateScrollListener;
        private LinearLayoutManager _layoutManager;
        private PublicationRecyclerViewAdapter _adapter;

        private PublicationItemViewHolder _previousPublicationViewHolder;
        private AutoFitTextureView _previousVideoView;

        private MvxInteraction _itemsChangedInteraction;
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
        }

        protected override void Unsubscription()
        {
            _publicationTypeTabLayout.TabSelected -= PublicationTypeTabLayoutTabSelected;
            _publicationTypeTabLayout.TabUnselected -= PublicationTypeTabLayoutTabUnselected;
            _stateScrollListener.FinishScroll -= StateScrollListenerFinishScroll;
            _itemsChangedInteraction.Requested -= OnDataSetChanged;
        }

        private void DoBind()
        {
            var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(this)
                      .For(v => v.ItemsChangedInteraction)
                      .To(vm => vm.ItemsChangedInteraction)
                      .OneWay();

            bindingSet.Bind(_publicationRecyclerView)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
        }

        private void InitializeControls(View view)
        {
            _publicationTypeTabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            _publicationRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.publication_recycler_view);

            _layoutManager = new SafeLinearLayoutManager(Context, LinearLayoutManager.Vertical, false)
            {
                InitialPrefetchItemCount = 40,
                MeasurementCacheEnabled = true,
                ItemPrefetchEnabled = true
            };

            _publicationRecyclerView.HasFixedSize = true;
            _publicationRecyclerView.SetLayoutManager(_layoutManager);
            _publicationRecyclerView.HasNextPage = true;

            _adapter = new PublicationRecyclerViewAdapter((IMvxAndroidBindingContext)BindingContext)
            {
                HasStableIds = true
            };

            _publicationRecyclerView.NestedScrollingEnabled = false;
            _publicationRecyclerView.SetAdapter(_adapter);

            _stateScrollListener = new StateScrollListener();
            _publicationRecyclerView.AddOnScrollListener(_stateScrollListener);

            var dividerItemDecoration = new DividerItemDecoration(Context, LinearLayoutManager.Vertical);
            _publicationRecyclerView.AddItemDecoration(dividerItemDecoration);

        }

        private void OnDataSetChanged(object sender, EventArgs e)
        {
            PlayVideoAfterReloadDataAsync().FireAndForget();
        }

        private async Task PlayVideoAfterReloadDataAsync()
        {
            await Task.Delay(MillisecondsDelay);
            await PlayVisibleVideoAsync();
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

            var viewHolder = _publicationRecyclerView.FindViewHolderForAdapterPosition(targetPosition);
            if (viewHolder is PublicationItemViewHolder itemViewHolder)
            {
                itemViewHolder.LoadingProgressBar.Visibility = ViewStates.Visible;
                PlayVideo(itemViewHolder, itemViewHolder.TextureView);
            }

            return Task.CompletedTask;
        }

        private void PlayVideo(PublicationItemViewHolder itemViewHolder, AutoFitTextureView textureView)
        {
            if (_previousPublicationViewHolder?.ViewModel != null &&
                _previousPublicationViewHolder.ViewModel.VideoPlayerService != null &&
                _previousVideoView != null)
            {
                StopVideo(_previousPublicationViewHolder);
            }

            Debug.WriteLine("PlayVideo [Start]");

            if (itemViewHolder?.ViewModel?.VideoPlayerService is null ||
                textureView is null)
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
            videoService.Play(itemViewHolder.ViewModel.VideoUrl, itemViewHolder.ViewModel.VideoId);

            _previousPublicationViewHolder = itemViewHolder;
            _previousVideoView = textureView;
            Debug.WriteLine("PlayVideo [End]");
        }

        private void StopVideo(PublicationItemViewHolder viewHolder)
        {
            Debug.WriteLine("StopVideo [Start]");
            if (viewHolder is null)
            {
                return;
            }

            viewHolder.ViewModel.VideoPlayerService.Player.VideoRenderingStartedAction = null;
            viewHolder.StubImageView.Visibility = ViewStates.Visible;
            viewHolder.ViewModel.VideoPlayerService.Stop();
            viewHolder.LoadingProgressBar.Visibility = ViewStates.Invisible;
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
