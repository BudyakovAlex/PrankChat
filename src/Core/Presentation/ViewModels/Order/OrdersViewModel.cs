using System;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        public MvxObservableCollection<OrderItemViewModel> Items { get; } = new MvxObservableCollection<OrderItemViewModel>();

        public OrdersViewModel(INavigationService navigationService) : base(navigationService)
        {
            Items.Add(new OrderItemViewModel(navigationService, "Подсесть к человеку в ТЦ и съесть его еду и сказать сальто де марто", "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg", "13 455 p", new DateTime(2019, 4, 22)));
            Items.Add(new OrderItemViewModel(navigationService, "Выпить бутылку воды без остановки", "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg", "995,55 p", new DateTime(2019, 11, 2)));
        }
    }
}
