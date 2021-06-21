namespace PrankChat.Mobile.Core.Models.Data
{
    public class CompetitionResult
    {
        public CompetitionResult(
            int place,
            User user,
            Video video,
            string prize)
        {
            Place = place;
            User = user;
            Video = video;
            Prize = prize;
        }

        public int Place { get; set; }

        public User User { get; set; }

        public Video Video { get; set; }

        public string Prize { get; set; }
    }
}