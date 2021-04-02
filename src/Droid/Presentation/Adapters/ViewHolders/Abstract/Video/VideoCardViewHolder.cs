using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Droid.Controls;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video
{
    public class VideoCardViewHolder<TViewModel> : CardViewHolder<TViewModel>, IVideoViewHolder
        where TViewModel : BaseVideoItemViewModel
    {
        public VideoCardViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        public View ProcessingView { get; private set; }

        public AutoFitTextureView TextureView { get; private set; }

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

        public IVideoPlayer VideoPlayer { get; set; }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            ProcessingView = view.FindViewById<View>(Resource.Id.processing_view);
            TextureView = view.FindViewById<AutoFitTextureView>(Resource.Id.texture_view);
            StubImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.stub_image_view);
            LoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);

            LoadingProgressBar.Visibility = ViewStates.Gone;
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<VideoCardViewHolder<TViewModel>, BaseVideoItemViewModel>();

            bindingSet.Bind(this).For(v => v.VideoPlayer).To(vm => vm.PreviewVideoPlayer);
        }

        public override void OnViewRecycled()
        {
            StubImageView.Visibility = ViewStates.Visible;
            LoadingProgressBar.Visibility = ViewStates.Invisible;

            base.OnViewRecycled();
        }
    }
}
