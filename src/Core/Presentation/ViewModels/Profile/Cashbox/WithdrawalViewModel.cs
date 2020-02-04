using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class WithdrawalViewModel : BaseViewModel
    {
        private string _cost;
        public string Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private string _availableForWithdrawal;
        public string AvailableForWithdrawal
        {
            get => _availableForWithdrawal;
            set => SetProperty(ref _availableForWithdrawal, value);
        }

        public List<PaymentMethodItemViewModel> Items { get; } = new List<PaymentMethodItemViewModel>();

        private PaymentMethodItemViewModel _selectedItem;
        public PaymentMethodItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ICommand SelectionChangedCommand => new MvxAsyncCommand<PaymentMethodItemViewModel>(OnSelectionChangedCommand);

        public ICommand WithdrawCommand => new MvxAsyncCommand<PaymentMethodItemViewModel>(OnWithdrawCommand);

        public WithdrawalViewModel(INavigationService navigationService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            AvailableForWithdrawal = $"{Resources.CashboxView_WithdrawalAvailable_Title} {"75 000"} {"₽"}";
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

        private Task OnWithdrawCommand(PaymentMethodItemViewModel arg)
        {
            return Task.CompletedTask;
        }

        private Task OnSelectionChangedCommand(PaymentMethodItemViewModel item)
        {
            SelectedItem = item;

            var items = Items.Where(c => c.IsSelected).ToList();
            items.ForEach(c => c.IsSelected = false);
            item.IsSelected = true;

            return Task.CompletedTask;
        }
    }
}
