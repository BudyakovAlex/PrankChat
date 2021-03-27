namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public interface IVideoItemViewModel : IFullScreenVideoOwnerViewModel
    {
        string VideoUrl { get; }

        string PreviewUrl { get; }

        string StubImageUrl { get; }

        int VideoId { get; }
    }
}
