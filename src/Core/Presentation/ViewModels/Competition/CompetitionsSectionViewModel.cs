﻿using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsSectionViewModel : BaseViewModel, IDisposable
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly INavigationService _navigationService;
        private readonly bool _isUserSessionInitialized;

        public CompetitionPhase Phase { get; }

        public List<CompetitionItemViewModel> Items { get; }

        public bool HasNavigationControls => Items.Count > 1;

        public CompetitionsSectionViewModel(bool isUserSessionInitialized,
                                            IMvxMessenger mvxMessenger,
                                            INavigationService navigationService,
                                            CompetitionPhase phase,
                                            List<Models.Data.Competition> competitions)
        {
            _isUserSessionInitialized = isUserSessionInitialized;
            _mvxMessenger = mvxMessenger;
            _navigationService = navigationService;

            Phase = phase;
            Items = competitions.Select(ProduceItemViewModel).ToList();
        }

        private CompetitionItemViewModel ProduceItemViewModel(Models.Data.Competition competition)
        {
            return new CompetitionItemViewModel(_isUserSessionInitialized, _mvxMessenger, _navigationService, competition);
        }
    }
}