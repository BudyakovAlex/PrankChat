﻿using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class PaymentMethodItemViewModel : BaseViewModel
    {
        public PaymentMethodItemViewModel(PaymentType paymentType)
        {
            Type = paymentType;
        }

        public PaymentType Type { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value, () => RaisePropertyChanged(nameof(Type)));
        }
    }
}