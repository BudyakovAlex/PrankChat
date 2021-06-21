using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionPrizePoolItemViewModel : BaseViewModel
    {
        public string Rating { get; }

        public string Participant { get; }

        public string Position { get; }

        public string Prize { get; set; }

        public bool IsMyPosition { get; }

        public CompetitionPrizePoolItemViewModel(string rating,
                                                 string participant,
                                                 string position,
                                                 string prize,
                                                 bool isMyPosition)
        {
            Rating = rating;
            Participant = participant;
            Position = position;
            Prize = prize;
            IsMyPosition = isMyPosition;
        }
    }
}