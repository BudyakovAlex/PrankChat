using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public interface IFullScreenVideoOwnerViewModel
    {
        FullScreenVideo GetFullScreenVideoDataModel();

        bool CanPlayVideo { get; }
    }
}
