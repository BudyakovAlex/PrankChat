﻿using PrankChat.Mobile.Core.BusinessServices;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public interface IVideoItemViewModel
    {
        IVideoPlayer VideoPlayer { get; }

        string VideoUrl { get; }

        string PreviewUrl { get; }

        string StubImageUrl { get; }

        int VideoId { get; }

        bool CanPlayVideo { get; }
    }
}