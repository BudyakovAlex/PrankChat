using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Presentation.Converters;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Droid.Presentation.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private FrameLayout _loadingOverlay;
        private MvxSwipeRefreshLayout _refreshView;
        private EndlessRecyclerView _recyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;

        private CompetitionVideoViewHolder _previousVideoViewHolder;
        private TextureView _previousVideoView;
        private StateScrollListener _stateScrollListener;
        private View _uploadingContainerView;
        private CircleProgressBar _uploadingProgressBar;
        private View _uploadingInfoContainer;
        private TextView _uploadedTextView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_competition_details);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _refreshView = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.competition_details_recycler_view);
            _layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionDetailsHeaderViewModel, CompetitionDetailsHeaderViewHolder>(Resource.Layout.cell_competition_details_header)
                .AddElement<CompetitionVideoViewModel, CompetitionVideoViewHolder>(Resource.Layout.cell_competition_video);

            _stateScrollListener = new StateScrollListener();
            _recyclerView.AddOnScrollListener(_stateScrollListener);

            _uploadingContainerView = FindViewById<View>(Resource.Id.uploading_progress_container);
            _uploadingProgressBar = FindViewById<CircleProgressBar>(Resource.Id.uploading_progress_bar);
            _uploadedTextView = FindViewById<TextView>(Resource.Id.uploaded_text_view);
            _uploadingInfoContainer = FindViewById<View>(Resource.Id.uploading_info_container);
            _uploadingInfoContainer.SetRoundedCorners(DisplayUtils.DpToPx(30) / 2);

            _uploadingProgressBar.ProgressColor = Color.White;
            _uploadingProgressBar.RingThickness = 5;
            _uploadingProgressBar.BaseColor = Color.Gray;
            _uploadingProgressBar.Progress = 0f;
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_refreshView)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsRefreshing);

            bindingSet.Bind(_refreshView)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshDataCommand);

            bindingSet.Bind(_recyclerView)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(_loadingOverlay)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsBusy)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Bind(_uploadingContainerView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsUploading);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.Progress)
                      .To(vm => vm.UploadingProgress);

            bindingSet.Bind(_uploadedTextView)
                      .For(v => v.Text)
                      .To(vm => vm.UploadingProgressStringPresentation);

            bindingSet.Bind(_uploadingProgressBar)
                      .For(v => v.BindClick())
                      .To(vm => vm.CancelUploadingCommand);

            bindingSet.Apply();
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
            if (viewHolder is CompetitionVideoViewHolder itemViewHolder)
            {
                itemViewHolder.LoadingProgressBar.Visibility = ViewStates.Visible;
                PlayVideo(itemViewHolder, itemViewHolder.TextureView);
            }

            return Task.CompletedTask;
        }

        private void PlayVideo(CompetitionVideoViewHolder itemViewHolder, TextureView textureView)
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

        private void StopVideo(CompetitionVideoViewHolder viewHolder)
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