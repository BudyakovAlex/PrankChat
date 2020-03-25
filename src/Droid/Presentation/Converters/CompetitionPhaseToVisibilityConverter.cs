using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class CompetitionPhaseToVisibilityConverter : MvxValueConverter<CompetitionPhase, ViewStates>
    {
        protected override ViewStates Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.New:
                    return ViewStates.Invisible;
                default:
                    return ViewStates.Visible;
            }
        }
    }
}