﻿using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class WithdrawalViewModel : BasePageViewModel
    {
        private readonly IMediaService _mediaService;

        public WithdrawalViewModel(IMediaService mediaService)
        {
            _mediaService = mediaService;
            AvailableForWithdrawal = $"{Resources.CashboxView_WithdrawalAvailable_Title} {SettingsService.User?.Balance.ToPriceString()}";

            WithdrawCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(WithdrawAsync));
            CancelWithdrawCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(CancelWithdrawAsync));
            AttachFileCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(AttachFileAsync));
            OpenCardOptionsCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(OpenCardOptionsAsync));
            UpdateDataCommand = new MvxAsyncCommand(UpdateDataAsync);
        }

        private CardDataModel _currentCard;
        private WithdrawalDataModel _lastWithdrawalDataModel;

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

        public DateTime? CreateAtWithdrawal => _lastWithdrawalDataModel?.CreatedAt;

        public string AmountValue => _lastWithdrawalDataModel?.Amount.ToPriceString();

        public bool IsAttachDocumentAvailable => SettingsService.User?.DocumentVerifiedAt == null && SettingsService.User?.Document == null;

        public bool IsDocumentPending => SettingsService.User?.DocumentVerifiedAt == null && SettingsService.User?.Document != null;

        public bool IsWithdrawalAvailable => !IsAttachDocumentAvailable && !IsDocumentPending && _lastWithdrawalDataModel == null;

        public bool IsWithdrawalPending => !IsAttachDocumentAvailable && !IsDocumentPending && _lastWithdrawalDataModel != null;

        public bool IsPresavedWithdrawalAvailable => _currentCard != null && IsWithdrawalAvailable;

        public ICommand WithdrawCommand { get; }

        public ICommand CancelWithdrawCommand { get; }

        public ICommand AttachFileCommand { get; }

        public ICommand OpenCardOptionsCommand { get; }

        public ICommand UpdateDataCommand { get; }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await GetUserCardAsync();
            await GetWithdrawalsAsync();
        }

        private async Task UpdateDataAsync()
        {
            try
            {
                IsUpdatingData = true;

                if (IsDocumentPending)
                {
                    await ApiService.GetCurrentUserAsync();
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
                    var card = await ApiService.SaveCardAsync(CardNumber, $"{Name} {Surname}");
                    if (card == null)
                    {
                        ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Empty_Card_Error, ValidationErrorType.CanNotMatch));
                        return;
                    }
                    _currentCard = card;
                    await RaiseAllPropertiesChanged();
                }

                var result = await ApiService.WithdrawalAsync(Cost.Value, _currentCard.Id);
                if (result != null)
                {
                    _lastWithdrawalDataModel = result;
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

                var document = await ApiService.SendVerifyDocumentAsync(file.Path);
                if (document != null)
                {
                    var user = SettingsService.User;
                    user.Document = document;
                    SettingsService.User = user;
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
            if (_lastWithdrawalDataModel == null)
            {
                 ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Cancel_Withdrawal_Error, ValidationErrorType.CanNotMatch, 0.ToString()));
                return;
            }

            try
            {
                await ApiService.CancelWithdrawalAsync(_lastWithdrawalDataModel.Id);
                _lastWithdrawalDataModel = null;
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
            _currentCard = await ApiService.GetCardsAsync();
        }

        private async Task GetWithdrawalsAsync()
        {
            var withdrawals = await ApiService.GetWithdrawalsAsync();
            _lastWithdrawalDataModel = withdrawals?.FirstOrDefault();
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

                await ApiService.DeleteCardAsync(_currentCard.Id);
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
                ErrorHandleService.HandleException(new ValidationException(Resources.WithdrawalView_Cancel_Withdrawal_Error, ValidationErrorType.CanNotMatch));
                return false;
            }

            return true;
        }
    }
}
