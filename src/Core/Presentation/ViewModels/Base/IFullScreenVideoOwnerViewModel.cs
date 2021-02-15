using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public interface IFullScreenVideoOwnerViewModel
    {
        FullScreenVideo GetFullScreenVideo();

        bool CanPlayVideo { get; }
    }
}
