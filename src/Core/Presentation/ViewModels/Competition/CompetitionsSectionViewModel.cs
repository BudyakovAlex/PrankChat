using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsSectionViewModel : BaseItemViewModel, IDisposable
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly INavigationService _navigationService;

        public CompetitionPhase Phase { get; }

        public List<CompetitionItemViewModel> Items { get; }

        public bool HasNavigationControls => Items.Count > 1;

        public CompetitionsSectionViewModel(IMvxMessenger mvxMessenger,
                                            INavigationService navigationService,
                                            CompetitionPhase phase,
                                            List<CompetitionDataModel> competitions)
        {
            _mvxMessenger = mvxMessenger;
            _navigationService = navigationService;

            Phase = phase;
            Items = competitions.Select(ProduceItemViewModel).ToList();
        }

        private CompetitionItemViewModel ProduceItemViewModel(CompetitionDataModel competition)
        {
            return new CompetitionItemViewModel(_mvxMessenger, _navigationService, competition);
        }

        public void Dispose()
        {
            Items.ForEach(item => item.Dispose());
        }
    }
}