using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionPrizePoolItemViewModel : BaseItemViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;

        public int Rating { get; set; }
        public string Participant { get; set; }
        public string Position { get; set; }

        public CompetitionPrizePoolItemViewModel(IMvxMessenger mvxMessenger, int rating, string participant, string position)
        {
            _mvxMessenger = mvxMessenger;
            Rating = rating;
            Participant = participant;
            Position = position;
        }
    }
}
