using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionPrizePoolItemViewModel : BaseItemViewModel
    {
        public int Rating { get; }
        public string Participant { get; }
        public string Position { get; }

        public CompetitionPrizePoolItemViewModel(int rating,
                                                 string participant,
                                                 string position)
        {
            Rating = rating;
            Participant = participant;
            Position = position;
        }
    }
}
