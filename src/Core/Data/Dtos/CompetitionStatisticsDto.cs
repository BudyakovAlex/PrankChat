using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CompetitionStatisticsDto
    {
        [JsonProperty("profit")]
        public decimal Profit { get; set; }

        [JsonProperty("participants")]
        public int Participants { get; set; }

        [JsonProperty("contribution")]
        public decimal Contribution { get; set; }

        [JsonProperty("percentage_from_contribution")]
        public int PercentageFromContribution { get; set; }
    }
}
