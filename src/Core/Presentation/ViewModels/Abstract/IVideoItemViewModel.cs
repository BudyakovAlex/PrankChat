﻿using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.Logger;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public interface IVideoItemViewModel : IFullScreenVideoOwnerViewModel
    {
        IVideoPlayerService VideoPlayerService { get; }

        ILogger Logger { get; }

        string VideoUrl { get; }

        string PreviewUrl { get; }

        string StubImageUrl { get; }

        int VideoId { get; }
    }
}
