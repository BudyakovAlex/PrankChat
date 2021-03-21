﻿using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class OrderDetailsNavigationParameter
    {
        public OrderDetailsNavigationParameter(
            int orderId,
            List<FullScreenVideo> fullScreenVideos,
            int currentIndex)
        {
            OrderId = orderId;
            FullScreenVideos = fullScreenVideos;
            CurrentIndex = currentIndex;
        }

        public int OrderId { get; }

        public List<FullScreenVideo> FullScreenVideos { get; }

        public int CurrentIndex { get; }
    }
}
