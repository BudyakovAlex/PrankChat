using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System.Linq;
using System.Diagnostics;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class RefillViewModel : BaseViewModel
    {
        private PaymentMethodItemViewModel _selectedItem;

        public RefillViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public List<PaymentMethodItemViewModel> Items { get; } = new List<PaymentMethodItemViewModel>();

        public PaymentMethodItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand SelectionChangedCommand
        {
            get => new MvxAsyncCommand<PaymentMethodItemViewModel>(OnSelectionChangedCommand);
        }

        private Task OnSelectionChangedCommand(PaymentMethodItemViewModel item)
        {
            SelectedItem = item;

            var items = Items.Where(c => c.IsSelected).ToList();
            items.ForEach(c => c.IsSelected = false);
            item.IsSelected = true;

            if (item.Type == PaymentType.Alphabank)
            {
                Debug.WriteLine("IS SELECTED: " + item.IsSelected);
            }

            return Task.CompletedTask;
        }

        public override Task Initialize()
        {
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Card));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Qiwi));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.YandexMoney));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Phone));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Sberbank));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Alphabank));

            return Task.CompletedTask;
        }
    }
}
