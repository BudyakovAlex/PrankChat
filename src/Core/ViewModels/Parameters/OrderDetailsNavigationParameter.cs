using PrankChat.Mobile.Core.ViewModels.Common.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Parameters
{
    public class OrderDetailsNavigationParameter
    {
        public OrderDetailsNavigationParameter(
            int orderId,
            BaseVideoItemViewModel[] fullScreenVideos,
            int currentIndex)
        {
            OrderId = orderId;
            FullScreenVideos = fullScreenVideos;
            CurrentIndex = currentIndex;
        }

        public int OrderId { get; }

        public BaseVideoItemViewModel[] FullScreenVideos { get; }

        public int CurrentIndex { get; }
    }
}