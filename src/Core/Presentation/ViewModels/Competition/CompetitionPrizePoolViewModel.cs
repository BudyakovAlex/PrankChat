using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionPrizePoolViewModel
    {
        public double PrizePool { get; set; }
        public ObservableCollection<CompetitionPrizePoolItemViewModel>  competitionPrizePoolsList { get; set; }
    }
}
