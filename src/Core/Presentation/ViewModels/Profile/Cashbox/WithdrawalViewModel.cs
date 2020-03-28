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
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class WithdrawalViewModel : BaseViewModel
    {
        private readonly IMediaService _mediaService;
        private CardDataModel _currentCard;

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
                //_cardNumber = value.VisualCardNumber();
                //RaisePropertyChanged(nameof(CardNumber));
                SetProperty(ref _cardNumber, value);
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        public ICommand WithdrawCommand => new MvxAsyncCommand(OnWithdrawAsync);

        public ICommand AttachFileCommand => new MvxAsyncCommand(OnAttachFileAsync);

        public ICommand DeleteCardCommand => new MvxAsyncCommand(OnDeleteCardAsync);

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

        public override Task Initialize()
        {
            return GetUserCard();
        }

        private async Task OnWithdrawAsync()
        {
            if (!CheckValidation())
                return;

            try
            {
                IsBusy = true;

                if (_currentCard == null)
                {
                    _currentCard = await ApiService.SaveCardAsync(CardNumber, $"{Name} {Surname}");
                }

                if (_currentCard == null)
                {
                    ErrorHandleService.HandleException(new ValidationException("Карта не может быть пустой", ValidationErrorType.CanNotMatch, 0.ToString()));
                    return;
                }

                var video = await ApiService.WithdrawalAsync(Cost.Value, _currentCard.Id);
                if (video != null)
                {
                    await RaiseAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                // TODO: Add the log.
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnAttachFileAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickPhotoAsync();
                if (file == null)
                    return;

                var document = await ApiService.SendVerifyDocumentAsync(file.Path);
                if (document != null)
                {
                    await RaiseAllPropertiesChanged();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnDeleteCardAsync()
        {
            if (_currentCard == null)
                return;

            try
            {
                IsBusy = true;

                await ApiService.DeleteCardAsync(_currentCard.Id);
                _currentCard = null;
            }
            catch (Exception ex)
            {
                // TODO: Add the log.
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetUserCard()
        {
            try
            {
                IsBusy = true;

                _currentCard = await ApiService.GetCardsAsync();
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
