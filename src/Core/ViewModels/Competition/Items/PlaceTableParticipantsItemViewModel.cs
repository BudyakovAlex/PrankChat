using System;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competition.Items
{
    public class PlaceTableParticipantsItemViewModel : BaseViewModel
    {
        private readonly int _place;
        private readonly Action? _percentChanged;
        private readonly Func<double> _leftToDistribute;

        public string Title => $"При за {_place} место";

        private double? _percent;
        public double? Percent
        {
            get => _percent;
            set => SetProperty(ref _percent, GetPercent(value), _percentChanged);
        }

        public PlaceTableParticipantsItemViewModel(int place, Func<double> leftToDistribute, Action? percentChanged)
        {
            _place = place;
            _percentChanged = percentChanged;
            _leftToDistribute = leftToDistribute;
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