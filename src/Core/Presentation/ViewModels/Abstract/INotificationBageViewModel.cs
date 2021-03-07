using MvvmCross.Commands;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public interface INotificationBageViewModel
    {
        IMvxAsyncCommand RefreshDataCommand { get; }

        bool HasUnreadNotifications { get; }
    }
}
