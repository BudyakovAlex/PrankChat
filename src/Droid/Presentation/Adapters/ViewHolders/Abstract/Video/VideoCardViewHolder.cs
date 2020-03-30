using Android.Media;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video
{
    public class VideoCardViewHolder<TViewModel> : CardViewHolder<TViewModel>
        where TViewModel : MvxNotifyPropertyChanged
    {
        public VideoCardViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            VideoView = view.FindViewById<VideoView>(Resource.Id.video_file);
            StubImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.stub_image_view);
            LoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            LoadingProgressBar.Visibility = ViewStates.Gone;
            VideoView.SetOnInfoListener(new MediaPlayerOnInfoListener(OnInfoChanged));
        }

        public VideoView VideoView { get; private set; }

        public MvxCachedImageView StubImageView { get; private set; }

        public ProgressBar LoadingProgressBar { get; private set; }

        public override void OnViewRecycled()
        {
            StubImageView.Visibility = ViewStates.Visible;
            LoadingProgressBar.Visibility = ViewStates.Gone;

            base.OnViewRecycled();
        }

        private bool OnInfoChanged(MediaPlayer mediaPlayer, MediaInfo mediaInfo, int extra)
        {
            if (mediaInfo == MediaInfo.VideoRenderingStart)
            {
                StubImageView.Visibility = ViewStates.Gone;
                LoadingProgressBar.Visibility = ViewStates.Gone;
            }

            return true;
        }
    }
}