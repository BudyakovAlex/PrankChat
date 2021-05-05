﻿using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Payment;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class WithdrawalViewModel : BasePageViewModel
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IUsersManager _usersManager;
        private readonly IMediaService _mediaService;

        private Card _currentCard;
        private Withdrawal _lastWithdrawal;

        public WithdrawalViewModel(
            IPaymentManager paymentManager,
            IUsersManager usersManager,
            IMediaService mediaService)
        {
            _paymentManager = paymentManager;
            _usersManager = usersManager;
            _mediaService = mediaService;

            WithdrawCommand = this.CreateCommand(WithdrawAsync);
            CancelWithdrawCommand = this.CreateCommand(CancelWithdrawAsync);
            AttachFileCommand = this.CreateCommand(AttachFileAsync);
            OpenCardOptionsCommand = this.CreateCommand(OpenCardOptionsAsync);
            UpdateDataCommand = this.CreateCommand(UpdateDataAsync);
        }

        private double? _cost;
        public double? Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public string AvailableForWithdrawal => GetWithdrawalAvailableTotalString();

        private string GetWithdrawalAvailableTotalString()
        {
            var balance = UserSessionProvider.User.Balance - UserSessionProvider.User?.Balance * Constants.Cashbox.PaymentServceCommissionMultiplier;
            return $"{Resources.CashboxView_WithdrawalAvailable_Title} {balance.ToPriceString()}";
        }

        private string _cardNumber;
        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                //check for delete symbol
                if (_cardNumber?.Length > 1 && value.Length > 1
                    && _cardNumber.Length > value.Length
                    && !_cardNumber[_cardNumber.Length - 1].IsDigit())
                {
                    _cardNumber = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _cardNumber = InternationalCardValidator.Instance.VisualCardNumber(value);
                }

                RaisePropertyChanged(nameof(CardNumber));
            }
        }

        public string CurrentCardNumber => InternationalCardValidator.Instance.VisualCardNumber(_currentCard?.Number);

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

        private bool _isUpdatingData;
        public bool IsUpdatingData
        {
            get => _isUpdatingData;
            set => SetProperty(ref _isUpdatingData, value);
        }

        private string _passport;
        public string Passport
        {
            get => _passport;
            set => SetProperty(ref _passport, value);
        }

        private string _nationality;
        public string Nationality
        {
            get => _nationality;
            set => SetProperty(ref _nationality, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set => SetProperty(ref _middleName, value);
        }

        public DateTime? CreateAtWithdrawal => _lastWithdrawal?.CreatedAt;

        public string AmountValue => _lastWithdrawal?.Amount.ToPriceString();

        public bool IsAttachDocumentAvailable => UserSessionProvider.User?.DocumentVerifiedAt == null && UserSessionProvider.User?.Document == null;

        public bool IsDocumentPending => UserSessionProvider.User?.DocumentVerifiedAt == null && UserSessionProvider.User?.Document != null;

        public bool IsWithdrawalAvailable => !IsAttachDocumentAvailable && !IsDocumentPending && _lastWithdrawal == null;

        public bool IsWithdrawalPending => !IsAttachDocumentAvailable && !IsDocumentPending && _lastWithdrawal != null;

        public bool IsPresavedWithdrawalAvailable => _currentCard != null && IsWithdrawalAvailable;

        public bool IsUserDataSaved => !UserSessionProvider.User?.IsPassportSaved ?? true;

        public ICommand WithdrawCommand { get; }

        public ICommand CancelWithdrawCommand { get; }

        public ICommand AttachFileCommand { get; }

        public ICommand OpenCardOptionsCommand { get; }

        public ICommand UpdateDataCommand { get; }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await GetUserCardAsync();

            await _usersManager.GetAndRefreshUserInSessionAsync();
            await GetWithdrawalsAsync();

            _ = RaisePropertyChanged(nameof(IsUserDataSaved));
        }

        private async Task UpdateDataAsync()
        {
            try
            {
                IsUpdatingData = true;

                if (IsDocumentPending)
                {
                    await _usersManager.GetAndRefreshUserInSessionAsync();
                    await RaiseAllPropertiesChanged();
                }
            }
            finally
            {
                IsUpdatingData = false;
            }
        }

        private async Task WithdrawAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            try
            {
                if (_currentCard == null)
                {
                    var card = await _usersManager.SaveCardAsync(CardNumber, $"{Name} {Surname}");
                    if (card == null)
                    {
                        ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Empty_Card_Error, ValidationErrorType.CanNotMatch));
                        return;
                    }
                    _currentCard = card;
                    await RaiseAllPropertiesChanged();
                }

                if (!IsUserDataSaved)
                {
                    await SendUserDataAsync();
                }

                var result = await _paymentManager.WithdrawalAsync(Cost.Value, _currentCard.Id);
                if (result != null)
                {
                    _lastWithdrawal = result;
                    await RaiseAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
            }
            finally
            {
                Messenger.Publish(new ReloadProfileMessage(this));
            }
        }

        private async Task AttachFileAsync()
        {
            try
            {
                var file = await _mediaService.PickPhotoAsync();
                if (file == null)
                {
                    return;
                }

                var document = await _usersManager.SendVerifyDocumentAsync(file.Path);
                if (document != null)
                {
                    var user = UserSessionProvider.User;
                    user.Document = document;
                    UserSessionProvider.User = user;
                    await RaiseAllPropertiesChanged();
                }
            }
            finally
            {
                Messenger.Publish(new ReloadProfileMessage(this));
            }
        }

        private async Task CancelWithdrawAsync()
        {
            if (_lastWithdrawal == null)
            {
                 ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Cancel_Withdrawal_Error, ValidationErrorType.CanNotMatch, 0.ToString()));
                return;
            }

            try
            {
                await _paymentManager.CancelWithdrawalAsync(_lastWithdrawal.Id);
                _lastWithdrawal = null;
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
            }
            finally
            {
                Messenger.Publish(new ReloadProfileMessage(this));
            }
        }

        private async Task OpenCardOptionsAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[] { Resources.WithdrawalView_Delete_Card_Text }, Resources.Close);
            if (result == Resources.WithdrawalView_Delete_Card_Text)
            {
                await DeleteCardAsync();
            }
        }

        private async Task GetUserCardAsync()
        {
            _currentCard = await _usersManager.GetCardsAsync();
        }

        private async Task GetWithdrawalsAsync()
        {
            var withdrawals = await _paymentManager.GetWithdrawalsAsync();
            _lastWithdrawal = withdrawals?.FirstOrDefault();
            await RaiseAllPropertiesChanged();
        }

        private async Task DeleteCardAsync()
        {
            if (_currentCard == null)
            {
                return;
            }

            try
            {
                var isConfirmed = await DialogService.ShowConfirmAsync(Resources.WithdrawalView_Delete_Card_Question, Resources.Attention, Resources.Delete, Resources.Cancel);
                if (!isConfirmed)
                {
                    return;
                }

                await _usersManager.DeleteCardAsync(_currentCard.Id);
                _currentCard = null;
                await RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
            }
            finally
            {
                Messenger.Publish(new ReloadProfileMessage(this));
            }
        }

        private bool CheckValidation()
        {
            if (Cost == null || Cost == 0)
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Validation_Field_Cost, ValidationErrorType.CanNotMatch, 0.ToString()));
                return false;
            }

            if (_currentCard == null && string.IsNullOrEmpty(CardNumber))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Empty_Card_Error, ValidationErrorType.CanNotMatch));
                return false;
            }

            return true;
        }

        //TODO: implement logic
        private Task SendUserDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}
