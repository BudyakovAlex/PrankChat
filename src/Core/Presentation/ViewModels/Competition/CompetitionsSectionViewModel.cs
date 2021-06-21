using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsSectionViewModel : BaseViewModel, IDisposable
    {
        private readonly bool _isUserSessionInitialized;

        public CompetitionPhase Phase { get; }

        public List<CompetitionItemViewModel> Items { get; }

        public bool HasNavigationControls => Items.Count > 1;

        public CompetitionsSectionViewModel(
            bool isUserSessionInitialized,
            CompetitionPhase phase,
            List<Models.Data.Competition> competitions)
        {
            _isUserSessionInitialized = isUserSessionInitialized;

            Phase = phase;
            Items = competitions.Select(ProduceItemViewModel).ToList();
        }

        private CompetitionItemViewModel ProduceItemViewModel(Models.Data.Competition competition)
        {
            return new CompetitionItemViewModel(_isUserSessionInitialized, competition);
        }
    }
}