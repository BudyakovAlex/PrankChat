using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IMvxLog _mvxLog;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly IErrorHandleService _errorHandleService;

        private DateTime? _completedDateValue;
        public DateTime? CompletedDateValue
        {
            get => _completedDateValue;
            set => SetProperty(ref _completedDateValue, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _currencySign;
        public string CurrencySign
        {
            get => _currencySign;
            set => _currencySign = value;
        }

        private long? _price;
        public long? Price
        {
            get => _price;
            set
            {
                //var price = value.EndsWith(_currencySign) ? value : value + _currencySign;
                SetProperty(ref _price, value);
            }
        }

        private bool _isExecutorHidden;
        public bool IsExecutorHidden
        {
            get => _isExecutorHidden;
            set => SetProperty(ref _isExecutorHidden, value);
        }

        public MvxAsyncCommand ShowDateDialogCommand => new MvxAsyncCommand(OnDateDialogAsync);

        public MvxAsyncCommand CreateCommand => new MvxAsyncCommand(OnCreateAsync);

        public CreateOrderViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IApiService apiService,
                                    IMvxLog mvxLog,
                                    IMvxMessenger mvxMessenger,
                                    ISettingsService settingsService,
                                    IErrorHandleService errorHandleService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
            _mvxMessenger = mvxMessenger;
            _settingsService = settingsService;
            _errorHandleService = errorHandleService;
        }

        public override void Prepare()
        {
            CurrencySign = "₽";
            base.Prepare();
        }

        private async Task OnCreateAsync()
        {
            if (!CheckValidation())
                return;

            try
            {
                IsBusy = true;

                var createOrderModel = new CreateOrderDataModel()
                {
                    Title = Title,
                    Description = Description,
                    AutoProlongation = IsExecutorHidden,
                    ActiveFor = (int) (CompletedDateValue.Value - DateTime.Now).TotalDays,
                    Price = Price.Value,
                };

                var newOrder = await _apiService.CreateOrderAsync(createOrderModel);
                if (newOrder != null)
                {
                    if (newOrder.Customer == null)
                        newOrder.Customer = _settingsService.User;

                    _mvxMessenger.Publish(new NewOrderMessage(this, newOrder));
                    await NavigationService.ShowDetailsOrderView(new OrderDetailsNavigationParameter(newOrder.Id));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnDateDialogAsync()
        {
            var result = await _dialogService.ShowArrayDialogAsync(new List<string>() { "LOL1", "LOL1", "LOL1", "LOL1", "LOL1", "LOL1", "LOL1", "LOL1", "LOL1", "LOL1"});
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                _errorHandleService.HandleException(new UserVisibleException("Название заказа не может быть пустым."));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                _errorHandleService.HandleException(new UserVisibleException("Описание заказа не может быть пустым."));
                return false;
            }

            if (Price == null)
            {
                _errorHandleService.HandleException(new UserVisibleException("Цена не может быть пустой."));
                return false;
            }

            if (Price <= 0)
            {
                _errorHandleService.HandleException(new UserVisibleException("Цена не может быть меньше или равна нулю."));
                return false;
            }

            if (CompletedDateValue == null)
            {
                _errorHandleService.HandleException(new UserVisibleException("Дата окончания не может быть пустой."));
                return false;
            }

            if (CompletedDateValue < DateTime.Now)
            {
                _errorHandleService.HandleException(new UserVisibleException("Дата окончания не может быть раньше текущей даты."));
                return false;
            }

            return true;
        }
    }
}
