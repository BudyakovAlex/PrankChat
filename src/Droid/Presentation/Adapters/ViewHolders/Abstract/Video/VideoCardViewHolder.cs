using Android.Media;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video
{
    public class VideoCardViewHolder<TViewModel> : CardViewHolder<TViewModel>
        where TViewModel : MvxNotifyPropertyChanged
    {
        public VideoCardViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        public View ProcessingView { get; private set; }

        public TextureView TextureView { get; private set; }

        public MvxCachedImageView StubImageView { get; private set; }

        public ProgressBar LoadingProgressBar { get; private set; }

        private bool _canShowStub = true;
        public bool CanShowStub
        {
            get => _canShowStub;
            set
            {
                _canShowStub = value;
                if (!value)
                {
                    StubImageView.Visibility = ViewStates.Invisible;
                }
            }
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            ProcessingView = view.FindViewById<View>(Resource.Id.processing_view);
            TextureView = view.FindViewById<TextureView>(Resource.Id.texture_view);
            StubImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.stub_image_view);
            LoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);

            LoadingProgressBar.Visibility = ViewStates.Gone;
        }

        public override void OnViewRecycled()
        {
            StubImageView.Visibility = ViewStates.Visible;
            LoadingProgressBar.Visibility = ViewStates.Invisible;

            base.OnViewRecycled();
        }

        public void OnRenderingStarted()
        {
            StubImageView.Visibility = ViewStates.Invisible;
            LoadingProgressBar.Visibility = ViewStates.Invisible;
        }
    }
}
