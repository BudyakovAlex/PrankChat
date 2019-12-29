using Android.Support.V7.Widget;
using Android.Views;
using MediaManager;
using MediaManager.Library;
using MediaManager.Platforms.Android.Media;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using VideoView = MediaManager.Platforms.Android.Video.VideoView;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    public class PublicationRecycleViewAdapter : MvxRecyclerAdapter
    {
        private readonly PublicationsViewModel _viewModel;
        private readonly IMvxBindingContext _bindingContext;

        public PublicationRecycleViewAdapter(PublicationsViewModel viewModel, IMvxBindingContext bindingContext)
        {
            _viewModel = viewModel;
            _bindingContext = bindingContext;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, ((IMvxAndroidBindingContext)_bindingContext).LayoutInflaterHolder);

            var viewHolder = new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext)
            {
                Id = viewType
            };

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            var videoView = holder.ItemView.FindViewById<VideoView>(Resource.Id.video_file);
            CrossMediaManager.Current.MediaPlayer.AutoAttachVideoView = false;
            CrossMediaManager.Current.MediaPlayer.VideoView = videoView;
            var mediaItem = new MediaItem(_viewModel.Items[position]?.VideoUrl);
            mediaItem.MediaType = MediaType.Video;
            mediaItem.DisplayDescription = "Some description";
            mediaItem.Title = "Some title";
            var info = mediaItem.ToMediaDescription();
            CrossMediaManager.Current.MediaPlayer.Play(mediaItem);
        }
    }
}
