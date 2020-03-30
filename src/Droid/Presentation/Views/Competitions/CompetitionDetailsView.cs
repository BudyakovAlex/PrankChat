using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity]
    public class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private EndlessRecyclerView _recyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;

        private CompetitionVideoViewHolder _previousVideoViewHolder;
        private VideoView _previousVideoView;
        private StateScrollListener _stateScrollListener;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_competition_details);
        }

        protected override void SetViewProperties()
        {
            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.competition_details_recycler_view);
            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionDetailsHeaderViewModel, CompetitionDetailsHeaderViewHolder>(Resource.Layout.cell_competition_details_header)
                .AddElement<CompetitionVideoViewModel, CompetitionVideoViewHolder>(Resource.Layout.cell_competition_video);

            _stateScrollListener = new StateScrollListener();
            _recyclerView.AddOnScrollListener(_stateScrollListener);
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

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
                PlayVideo(itemViewHolder, itemViewHolder.VideoView);
            }

            return Task.CompletedTask;
        }

        private void PlayVideo(CompetitionVideoViewHolder itemViewHolder, VideoView videoView)
        {
            if (_previousVideoViewHolder?.ViewModel != null &&
                _previousVideoViewHolder.ViewModel.VideoPlayerService != null &&
                _previousVideoView != null)
            {
                StopVideo(_previousVideoViewHolder);
                _previousVideoView.SetBackgroundColor(Color.Black);
            }

            Debug.WriteLine("PlayVideo [Start]");

            if (itemViewHolder?.ViewModel?.VideoPlayerService is null ||
                videoView is null ||
                itemViewHolder?.ViewModel?.VideoUrl is null)
            {
                return;
            }

            if (itemViewHolder.ViewModel.VideoPlayerService.Player.IsPlaying)
            {
                return;
            }

            var videoService = itemViewHolder.ViewModel.VideoPlayerService;

            videoView.SetBackgroundColor(Color.Transparent);
            videoService.Player.SetPlatformVideoPlayerContainer(videoView);
            videoService.Play(itemViewHolder.ViewModel.VideoUrl, itemViewHolder.ViewModel.VideoId);
            _previousVideoViewHolder = itemViewHolder;
            _previousVideoView = videoView;

            Debug.WriteLine("PlayVideo [End]");
        }

        private void StopVideo(CompetitionVideoViewHolder viewHolder)
        {
            Debug.WriteLine("StopVideo [Start]");
            if (viewHolder is null)
            {
                return;
            }

            viewHolder.StubImageView.Visibility = ViewStates.Visible;
            viewHolder.ViewModel.VideoPlayerService.Stop();
        }
    }
}