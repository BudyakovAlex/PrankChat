using System;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrdersViewModel : BaseViewModel
    {
        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public OrdersViewModel(INavigationService navigationService) : base(navigationService)
        {
            Items.Add(new PublicationItemViewModel());
            Items.Add(new PublicationItemViewModel());
        }
    }
}
