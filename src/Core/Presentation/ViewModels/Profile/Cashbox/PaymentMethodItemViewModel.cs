namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class PaymentMethodItemViewModel : BaseItemViewModel
    {
        private bool _isSelected;

        public PaymentType Type { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    RaisePropertyChanged(nameof(Type));
                }
            }
        }

        public PaymentMethodItemViewModel(PaymentType paymentType)
        {
            Type = paymentType;
        }
    }
}
