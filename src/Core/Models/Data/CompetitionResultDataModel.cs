namespace PrankChat.Mobile.Core.Models.Data
{
    public class CompetitionResultDataModel
    {
        public int Place { get; set; }

        public UserDataModel User { get; set; }

        public VideoDataModel Video { get; set; }

        public string Prize { get; set; }
    }
}