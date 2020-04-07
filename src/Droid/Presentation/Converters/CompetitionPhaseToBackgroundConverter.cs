﻿using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class CompetitionPhaseToBackgroundConverter : MvxValueConverter<CompetitionPhase, int>
    {
        protected override int Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.Voting:
                    return Resource.Drawable.bg_competition_orange;
                case CompetitionPhase.New:
                    return Resource.Drawable.bg_competition_blue;
                case CompetitionPhase.Finished:
                    return Resource.Drawable.bg_competition_gray;
                default:
                    return 0;
            }
        }
    }
}