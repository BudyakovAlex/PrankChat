using Android.Views;
using AndroidX.RecyclerView.Widget;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners
{
    public class VideoRecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        private IVideoViewHolder _previousVideoViewHolder;

        private SafeLinearLayoutManager _layoutManager;

        public VideoRecyclerViewScrollListener(SafeLinearLayoutManager layoutManager)
        {
            _layoutManager = layoutManager;
        }

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            base.OnScrollStateChanged(recyclerView, newState);
            if (newState != RecyclerView.ScrollStateIdle)
            {
                return;
            }

            _ = PlayVisibleVideoAsync(recyclerView);
        }

        public Task PlayVisibleVideoAsync(RecyclerView recyclerView)
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

            var viewHolder = recyclerView.FindViewHolderForAdapterPosition(targetPosition);
            if (viewHolder is IVideoViewHolder itemViewHolder)
            {
                itemViewHolder.ShowStubAndLoader();
                PlayVideo(itemViewHolder);
            }

            return Task.CompletedTask;
        }

        private void PlayVideo(IVideoViewHolder itemViewHolder)
        {
            StopVideo(_previousVideoViewHolder);
    
            Debug.WriteLine("PlayVideo [Start]");

            if (!(itemViewHolder?.VideoPlayer is VideoPlayer videoPlayer))
            {
                return;
            }

            videoPlayer.SetPlatformVideoPlayerContainer(itemViewHolder.TextureView);
            videoPlayer.Play();

            _previousVideoViewHolder = itemViewHolder;
        }

        private void StopVideo(IVideoViewHolder viewHolder)
        {
            Debug.WriteLine("StopVideo [Stop]");
            if (viewHolder?.VideoPlayer is null)
            {
                return;
            }

            viewHolder.StubImageView.Visibility = ViewStates.Visible;
            viewHolder.LoadingProgressBar.Visibility = ViewStates.Invisible;
            viewHolder.VideoPlayer.Stop();
        }
    }
}