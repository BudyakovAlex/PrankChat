using Android.Widget;
using FFImageLoading.Cross;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.Controls;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video
{
    public interface IVideoViewHolder
    {
        IVideoPlayer VideoPlayer { get; set; }

        AutoFitTextureView TextureView { get; }

        MvxCachedImageView StubImageView { get; }

        ProgressBar LoadingProgressBar { get; }
    }
}
