namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class FullScreenVideoParameter
    {
        public string VideoUrl { get; }

        public string VideoName { get; }

        public string Description { get; }

        public FullScreenVideoParameter(string videoUrl, string videoName, string description)
        {
            VideoUrl = videoUrl;
            VideoName = videoName;
            Description = description;
        }
    }
}