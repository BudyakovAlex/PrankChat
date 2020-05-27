using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Forbidden;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly ISettingsService _settingsService;
        private readonly IWalkthroughsProvider _walkthroughsProvider;
        private bool _isExecuting;

        private PeriodDataModel _activeFor;
        public PeriodDataModel ActiveFor
        {
            get => _activeFor;
            set => SetProperty(ref _activeFor, value);
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

        private double? _price;
        public double? Price
        {
            get => _price;
            set
            {
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

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public CreateOrderViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IApiService apiService,
                                    IMvxMessenger mvxMessenger,
                                    ISettingsService settingsService,
                                    IErrorHandleService errorHandleService,
                                    IWalkthroughsProvider walkthroughsProvider)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _settingsService = settingsService;
            _walkthroughsProvider = walkthroughsProvider;

            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<CreateOrderViewModel>();
        }

        private async Task OnCreateAsync()
        {
            if (_isExecuting)
            {
                return;
            }

            _isExecuting = true;

            if (!CheckValidation())
            {
                _isExecuting = false;
                return;
            }

            try
            {
                var canCreate = await DialogService.ShowConfirmAsync(Resources.Order_Create_Message, Resources.Attention, Resources.Order_Add, Resources.Cancel);
                if (!canCreate)
                {
                    return;
                }

                IsBusy = true;

                var createOrderModel = new CreateOrderDataModel()
                {
                    Title = Title,
                    Description = Description,
                    AutoProlongation = IsExecutorHidden,
                    ActiveFor = ActiveFor?.Hours ?? 0,
                    Price = Price.Value,
                };

                ErrorHandleService.SuspendServerErrorsHandling();
                var newOrder = await ApiService.CreateOrderAsync(createOrderModel);
                if (newOrder != null)
                {
                    if (newOrder.Customer == null)
                    {
                        newOrder.Customer = _settingsService.User;
                    }

                    _mvxMessenger.Publish(new OrderChangedMessage(this, newOrder));
                    await NavigationService.ShowOrderDetailsView(newOrder.Id);
                    SetDefaultData();
                }
            }
            catch (Exception ex)
            {
                await HandleLowBalanceExceptionAsync(ex);
            }
            finally
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                _isExecuting = false;
                IsBusy = false;
            }
        }

        private async Task HandleLowBalanceExceptionAsync(Exception exception)
        {
            var canRefil = await DialogService.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.ProfileView_Refill, Resources.Cancel);
            if (!canRefil)
            {
                return;
            }

            await NavigationService.ShowRefillView();
        }

        private async Task OnDateDialogAsync()
        {
            var periods = ConfigurationProvider.GetConfiguration().Periods;
            var titles = periods.Select(period => period.Title).ToList();
            var result = await DialogService.ShowArrayDialogAsync(titles, Resources.CreateOrderView_Choose_Time_Period);
            if (!string.IsNullOrWhiteSpace(result))
            {
                ActiveFor = periods.FirstOrDefault(p => p.Title == result);
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Title, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Title can't be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Description, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Description can't be empty.");
                return false;
            }

            if (Price == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Price, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Price can't be empty.");
                return false;
            }

            if (Price <= 0)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Price, ValidationErrorType.LowerThanRequired, 0.ToString()));
                ErrorHandleService.LogError(this, "Description can't be lower than zero.");
                return false;
            }

            if (ActiveFor == null)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_OrderPeriod, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "Order period can't be empty.");
                return false;
            }

            return true;
        }

        private void SetDefaultData()
        {
            Title = string.Empty;
            Description = string.Empty;
            IsExecutorHidden = false;
            ActiveFor = null;
            Price = null;
        }
    }
}
