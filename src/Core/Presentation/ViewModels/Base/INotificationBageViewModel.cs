using MvvmCross.Commands;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public interface INotificationBageViewModel
    {
        IMvxAsyncCommand RefreshDataCommand { get; }

        bool HasUnreadNotifications { get; }
    }
}
