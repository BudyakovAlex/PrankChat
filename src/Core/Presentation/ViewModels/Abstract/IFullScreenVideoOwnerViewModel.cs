using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public interface IFullScreenVideoOwnerViewModel
    {
        FullScreenVideo GetFullScreenVideo();

        bool CanPlayVideo { get; }
    }
}
