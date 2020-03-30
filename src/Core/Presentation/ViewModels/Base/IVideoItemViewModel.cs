using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public interface IVideoItemViewModel
    {
        IVideoPlayerService VideoPlayerService { get; }

        string VideoUrl { get; }

        int VideoId { get; }
    }
}
