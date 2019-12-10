using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification
{
    public class NotificationViewModel : BaseViewModel
    {
        public MvxObservableCollection<NotificationItemViewModel> Items { get; } = new MvxObservableCollection<NotificationItemViewModel>();

        public NotificationViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override Task Initialize()
        {
            Items.Add(new NotificationItemViewModel("Ann", "Выполнено Задание Example", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", new DateTime(2019, 4, 12), "Непросмотрено"));
            Items.Add(new NotificationItemViewModel("Kirill", "Выполнено Задание Example. Выполнено Задание Example", "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500", new DateTime(2019, 9, 24), ""));
            return base.Initialize();
        }
    }
}
