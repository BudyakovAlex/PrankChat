using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionPrizePoolItemViewModel : BaseItemViewModel
    {
        public string Rating { get; }
        public string Participant { get; }
        public int Position { get; }

        public CompetitionPrizePoolItemViewModel(string rating,
                                                 string participant,
                                                 int position)
        {
            Rating = rating;
            Participant = participant;
            Position = position;
        }
    }
}
