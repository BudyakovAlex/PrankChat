namespace PrankChat.Mobile.Core.Models.Data
{
    public class FullScreenVideoDataModel
    {
        public FullScreenVideoDataModel(int videoId,
                                        string videoUrl,
                                        string videoName,
                                        string description,
                                        string shareLink,
                                        string profilePhotoUrl,
                                        long? numberOfLikes,
                                        long? numberOfComments,
                                        bool isLiked,
                                        bool isLikeFlowAvailable = true)
        {
            VideoId = videoId;
            VideoUrl = videoUrl;
            VideoName = videoName;
            Description = description;
            ShareLink = shareLink;
            ProfilePhotoUrl = profilePhotoUrl;
            NumberOfLikes = numberOfLikes;
            NumberOfComments = numberOfComments;
            IsLiked = isLiked;
            IsLikeFlowAvailable = isLikeFlowAvailable;
        }

        public int VideoId { get; }

        public string VideoUrl { get; }

        public string VideoName { get; }

        public string Description { get; }

        public string ShareLink { get; }

        public string ProfilePhotoUrl { get; }

        public long? NumberOfLikes { get; }

        public long? NumberOfComments { get; }

        public bool IsLiked { get; }

        public bool IsLikeFlowAvailable { get; }
    }
}