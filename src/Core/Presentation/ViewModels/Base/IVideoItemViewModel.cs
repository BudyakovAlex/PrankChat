using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.Logger;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public interface IVideoItemViewModel : IFullScreenVideoOwnerViewModel
    {
        IVideoPlayerService VideoPlayerService { get; }

        ILogger Logger { get; }

        string VideoUrl { get; }

        string PreviewUrl { get; }

        int VideoId { get; }
    }
}
