using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CreditCardValidator;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class WithdrawalViewModel : BaseViewModel
    {
        private readonly IMediaService _mediaService;
        private bool _isNewCard;

        private double? _cost;
        public double? Cost
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

        private string _cardNumber;
        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                _cardNumber = value.VisualCardNumber();
                RaisePropertyChanged(nameof(CardNumber));
                //SetProperty(ref _cardNumber, value);
            }
        }

        public ICommand WithdrawCommand => new MvxAsyncCommand(OnWithdrawAsync);

        public ICommand AttachFileCommand => new MvxAsyncCommand(OnAttachFileAsync);

        public WithdrawalViewModel(INavigationService navigationService,
                                   IErrorHandleService errorHandleService,
                                   IApiService apiService,
                                   IDialogService dialogService,
                                   ISettingsService settingsService,
                                   IMediaService mediaService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mediaService = mediaService;
            AvailableForWithdrawal = $"{Resources.CashboxView_WithdrawalAvailable_Title} {settingsService.User?.Balance.ToPriceString()}";
        }

        private Task OnWithdrawAsync()
        {
            if (!CheckValidation())
                return Task.CompletedTask;

            return Task.CompletedTask;
        }

        private async Task OnAttachFileAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickPhotoAsync();
                if (file == null)
                    return;

                var video = await ApiService.SendVerifyDocumentAsync(file.Path);
                if (video != null)
                {
                    await RaiseAllPropertiesChanged();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CheckValidation()
        {
            if (Cost == null || Cost == 0)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Cost, ValidationErrorType.CanNotMatch, 0.ToString()));
                return false;
            }

            if (string.IsNullOrEmpty(CardNumber))// || (!_isNewCard && ))
            {
                ErrorHandleService.HandleException(new ValidationException("Карта не может быть пустой", ValidationErrorType.CanNotMatch, 0.ToString()));
                return false;
            }

            return true;
        }
    }
}
