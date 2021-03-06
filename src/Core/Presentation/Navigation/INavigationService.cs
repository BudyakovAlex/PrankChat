using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public interface INavigationService
    {
        Task ShowLoginView();

        Task ShowMainView();

        Task<OrderDetailsResult> ShowOrderDetailsView(int orderId, List<FullScreenVideo> fullScreenVideos, int currentIndex);

        Task<bool> ShowUserProfile(int userId);

        Task AppStartFromNotification(int orderId);
    }
}
