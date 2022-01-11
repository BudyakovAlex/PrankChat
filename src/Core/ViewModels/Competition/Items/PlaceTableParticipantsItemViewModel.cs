using System;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competition.Items
{
    public class PlaceTableParticipantsItemViewModel : BaseViewModel
    {
        private readonly Action _percentChanged;
        private readonly Func<double> _leftToDistribute;

        public PlaceTableParticipantsItemViewModel(
            int place,
            Func<double> leftToDistribute,
            Action percentChanged)
        {
            Place = place;
            _percentChanged = percentChanged;
            _leftToDistribute = leftToDistribute;
        }

        public int Place { get; }

        public string Title => $"Приз за {Place} место";

        private double? _percent;
        public double? Percent
        {
            get => _percent;
            set
            {
                _percent = value;
                _percentChanged?.Invoke();

                var availablePercent = GetPercent(value);
                if (availablePercent < _percent)
                {
                    Percent = availablePercent;
                }

                RaisePropertyChanged();
            }
        }

        private double GetPercent(double? value)
        {
            if (value == null)
            {
                return 0;
            }

            return Math.Min(value ?? 0, _leftToDistribute.Invoke());
        }
    }
}