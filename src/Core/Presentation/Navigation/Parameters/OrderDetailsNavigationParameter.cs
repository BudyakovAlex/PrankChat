using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class OrderDetailsNavigationParameter
    {
        public int OrderId { get; }

        public List<FullScreenVideoDataModel> FullScreenVideos { get; }

        public int CurrentIndex { get; }

        public OrderDetailsNavigationParameter(int orderId,
                                               List<FullScreenVideoDataModel> fullScreenVideos,
                                               int currentIndex)
        {
            OrderId = orderId;
            FullScreenVideos = fullScreenVideos;
            CurrentIndex = currentIndex;
        }
    }
}
