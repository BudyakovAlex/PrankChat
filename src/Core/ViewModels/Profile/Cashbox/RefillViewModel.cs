using MvvmCross.Commands;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Payment;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Profile.Cashbox
{
    public class RefillViewModel : BasePageViewModel
    {
        private readonly IPaymentManager _paymentManager;

        public RefillViewModel(IPaymentManager paymentManager)
        {
            _paymentManager = paymentManager;

            Items = new List<PaymentMethodItemViewModel>();
            RefillCommand = this.CreateCommand(OnRefillAsync);
            SelectionChangedCommand = this.CreateCommand<PaymentMethodItemViewModel>(OnSelectionChangedAsync);
        }

        private double? _cost;
        public double? Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public List<PaymentMethodItemViewModel> Items { get; } 

        private PaymentMethodItemViewModel _selectedItem;
        public PaymentMethodItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public IMvxAsyncCommand<PaymentMethodItemViewModel> SelectionChangedCommand { get; }

        public IMvxAsyncCommand RefillCommand { get; }

        public override Task InitializeAsync()
        {
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Card));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Qiwi));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.YandexMoney));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Phone));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Sberbank));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Alphabank));

            return base.InitializeAsync();
        }

        private async Task OnRefillAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            var paymentData = await _paymentManager.RefillAsync(Cost.Value);
            if (string.IsNullOrWhiteSpace(paymentData?.PaymentLink))
            {
                ErrorHandleService.LogError(this, "Can't resolve payment link, payment process aborted.");
                return;
            }

            await NavigationManager.NavigateAsync<WebViewModel, string>(paymentData.PaymentLink);
            Messenger.Publish(new ReloadProfileMessage(this));
        }

        private Task OnSelectionChangedAsync(PaymentMethodItemViewModel item)
        {
            SelectedItem = item;

            var items = Items.Where(paymentMethod => paymentMethod.IsSelected).ToList();
            items.ForEach(paymentMethod => paymentMethod.IsSelected = false);
            item.IsSelected = true;

            return Task.CompletedTask;
        }

        private bool CheckValidation()
        {
            if (Cost == null || Cost == 0)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Cost, ValidationErrorType.CanNotMatch, 0.ToString()));
                return false;
            }

            return true;
        }
    }
}
