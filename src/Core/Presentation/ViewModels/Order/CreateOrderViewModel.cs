using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;

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

        private long _price;
        public long Price
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
                                    IApiService apiService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
        }

        public override void Prepare()
        {
            CurrencySign = "₽";
            base.Prepare();
        }

        private async Task OnCreateAsync()
        {
            try
            {
                var createOrderModel = new CreateOrderDataModel()
                {
                    Title = Title,
                    Description = Description,
                    AutoProlongation = IsExecutorHidden,
                    ActiveTo = CompletedDateValue.Value,
                    Price = Price,
                };
                await _apiService.CreateOrderAsync(createOrderModel);

                _dialogService.ShowToastAsync("Order is created");
            }
            catch (Exception ex)
            {
                _dialogService.ShowToastAsync("Required data is empty");
            }
        }

        private async Task OnDateDialogAsync()
        {
            var result = await _dialogService.ShowDateDialogAsync();
            if (result.HasValue)
            {
                CompletedDateValue = result.Value;
            }
        }
    }
}
