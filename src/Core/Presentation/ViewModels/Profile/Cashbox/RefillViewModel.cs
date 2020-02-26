using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System.Linq;
using MvvmCross.Logging;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using System;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class RefillViewModel : BaseViewModel
    {
        private double? _cost;
        public double? Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public List<PaymentMethodItemViewModel> Items { get; } = new List<PaymentMethodItemViewModel>();

        private PaymentMethodItemViewModel _selectedItem;
        public PaymentMethodItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public MvxAsyncCommand<PaymentMethodItemViewModel> SelectionChangedCommand => new MvxAsyncCommand<PaymentMethodItemViewModel>(OnSelectionChangedAsync);

        public MvxAsyncCommand RefillCommand => new MvxAsyncCommand(OnRefillAsync);

        public RefillViewModel(INavigationService navigationService,
                               IErrorHandleService errorHandleService,
                               IApiService apiService,
                               IDialogService dialogService,
                               ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public override Task Initialize()
        {
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Card));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Qiwi));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.YandexMoney));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Phone));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Sberbank));
            Items.Add(new PaymentMethodItemViewModel(PaymentType.Alphabank));

            return base.Initialize();
        }

        private async Task OnRefillAsync()
        {
            if (!CheckValidation())
                return;

            var paymentData = await ApiService.RefillAsync(Cost.Value);
            if (string.IsNullOrWhiteSpace(paymentData?.PaymentLink))
            {
<<<<<<< HEAD
                ErrorHandleService.HandleException(new UserVisibleException(Resources.Error_Cost_Not_Empty));
=======
                ErrorHandleService.LogError(this, "Can't resolve payment link, payment process aborted.");
>>>>>>> development
                return;
            }

            await NavigationService.ShowWebView(paymentData.PaymentLink);
        }

        private Task OnSelectionChangedAsync(PaymentMethodItemViewModel item)
        {
            SelectedItem = item;

            var items = Items.Where(c => c.IsSelected).ToList();
            items.ForEach(c => c.IsSelected = false);
            item.IsSelected = true;

            return Task.CompletedTask;
        }

        private bool CheckValidation()
        {
            if (Cost == null || Cost == 0)
            {
<<<<<<< HEAD
                ErrorHandleService.HandleException(new UserVisibleException(Resources.Error_Cost_Not_Empty));
=======
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Cost, ValidationErrorType.CanNotMatch, 0.ToString()));
>>>>>>> development
                return false;
            }

            return true;
        }
    }
}
