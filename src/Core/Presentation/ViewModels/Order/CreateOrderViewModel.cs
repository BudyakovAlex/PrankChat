﻿using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private bool _isExecuting;

        public CreateOrderViewModel(IWalkthroughsProvider walkthroughsProvider)
        {
            _walkthroughsProvider = walkthroughsProvider;

            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
            ShowDateDialogCommand = new MvxAsyncCommand(ShowDateDialogAsync);
            CreateCommand = new MvxAsyncCommand(CreateAsync);
        }

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

        public IMvxAsyncCommand ShowDateDialogCommand { get; }

        public IMvxAsyncCommand CreateCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();
            base.ViewDestroy(viewFinishing);
        }

        private void Subscription()
        {
            SubscribeToNotificationsUpdates();
        }

        private void Unsubscription()
        {
            UnsubscribeFromNotificationsUpdates();
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<CreateOrderViewModel>();
        }

        private async Task CreateAsync()
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

                await SaveOrderAsync();
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsDataModel problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.LowBalance)
            {
                await HandleLowBalanceExceptionAsync(ex);
            }
            catch (Exception ex)
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                Messenger.Publish(new ServerErrorMessage(this, ex));
            }
            finally
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                _isExecuting = false;
                IsBusy = false;
            }
        }

        private async Task SaveOrderAsync()
        {
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
                    newOrder.Customer = SettingsService.User;
                }

                Messenger.Publish(new OrderChangedMessage(this, newOrder));
                await NavigationService.ShowOrderDetailsView(newOrder.Id, null, 0);
                SetDefaultData();
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

        private async Task ShowDateDialogAsync()
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
