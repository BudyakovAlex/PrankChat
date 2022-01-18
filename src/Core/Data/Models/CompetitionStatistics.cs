namespace PrankChat.Mobile.Core.Data.Models
{
    public class CompetitionStatistics
    {
        public CompetitionStatistics(
            decimal profit,
            int participants,
            decimal contribution,
            int percentageFromContribution)
        {
            Profit = profit;
            Participants = participants;
            Contribution = contribution;
            PercentageFromContribution = percentageFromContribution;
        }

        public decimal Profit { get; }

        public int Participants { get; }

        public decimal Contribution { get; }

        public int PercentageFromContribution { get; }
    }
}