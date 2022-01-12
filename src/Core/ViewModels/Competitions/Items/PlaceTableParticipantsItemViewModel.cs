using System;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competitions.Items
{
    public class PlaceTableParticipantsItemViewModel : BaseViewModel
    {
        private const int MaximumPercents = 100;

        private readonly Action _percentChanged;
        private readonly Func<double> _getTotalPercentsInInput;

        public PlaceTableParticipantsItemViewModel(
            int place,
            Func<double> getTotalPercentsInInput,
            Action percentChanged)
        {
            Place = place;
            _percentChanged = percentChanged;
            _getTotalPercentsInInput = getTotalPercentsInInput;
        }

        public int Place { get; }

        public string Title => string.Format(Resources.PrizeForPlaceTemplate, Place);

        private double? _percent;
        public double? Percent
        {
            get => _percent;
            set
            {
                _percent = value;
                var availablePercent = GetMaxAvailablePercent(value);
                SetProperty(ref _percent, availablePercent);
                _percentChanged?.Invoke();
             }
        }

        private double GetMaxAvailablePercent(double? value)
        {
            if (value == null)
            {
                return 0;
            }

            var totalPercents = _getTotalPercentsInInput.Invoke();
            if (totalPercents <= MaximumPercents)
            {
                return value.Value;
            }

            return MaximumPercents - (totalPercents - value ?? 0);
        }
    }
}