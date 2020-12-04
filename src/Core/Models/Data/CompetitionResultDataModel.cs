namespace PrankChat.Mobile.Core.Models.Data
{
    public class CompetitionResultDataModel
    {
        public CompetitionResultDataModel(int place,
                                          UserDataModel user,
                                          VideoDataModel video,
                                          string prize)
        {
            Place = place;
            User = user;
            Video = video;
            Prize = prize;
        }

        public int Place { get; set; }

        public UserDataModel User { get; set; }

        public VideoDataModel Video { get; set; }

        public string Prize { get; set; }
    }
}