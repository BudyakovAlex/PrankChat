namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
{
    public class FullScreenVideoParameter
    {
        public int VideoId { get; }

        public string VideoUrl { get; }

        public string VideoName { get; }

        public string Description { get; }

        public string ShareLink { get; }

        public string ProfilePhotoUrl { get; set; }

        public FullScreenVideoParameter(int videoId,
                                        string videoUrl,
                                        string videoName,
                                        string description,
                                        string shareLink,
                                        string profilePhotoUrl)
        {
            VideoId = videoId;
            VideoUrl = videoUrl;
            VideoName = videoName;
            Description = description;
            ShareLink = shareLink;
            ProfilePhotoUrl = profilePhotoUrl;
        }
    }
}