namespace PrankChat.Mobile.Core.Models.Data
{
    public class FullScreenVideoDataModel
    {
        public FullScreenVideoDataModel(int userId,
                                        bool isSubscribed,
                                        int videoId,
                                        string videoUrl,
                                        string videoName,
                                        string description,
                                        string shareLink,
                                        string profilePhotoUrl,
                                        string userShortName,
                                        long? numberOfLikes,
                                        long? numberOfDislikes,
                                        long? numberOfComments,
                                        bool isLiked,
                                        bool isDisliked,
                                        bool isLikeFlowAvailable = true)
        {
            UserId = userId;
            IsSubscribed = isSubscribed;
            VideoId = videoId;
            VideoUrl = videoUrl;
            VideoName = videoName;
            Description = description;
            ShareLink = shareLink;
            ProfilePhotoUrl = profilePhotoUrl;
            UserShortName = userShortName;
            NumberOfLikes = numberOfLikes;
            NumberOfDislikes = numberOfDislikes;
            NumberOfComments = numberOfComments;
            IsLiked = isLiked;
            IsDisliked = isDisliked;
            IsLikeFlowAvailable = isLikeFlowAvailable;
        }

        public bool IsSubscribed { get; set; }

        public int UserId { get; }

        public int VideoId { get; }

        public string VideoUrl { get; }

        public string VideoName { get; }

        public string Description { get; }

        public string ShareLink { get; }

        public string ProfilePhotoUrl { get; }

        public string UserShortName { get; }

        public long? NumberOfLikes { get; }

        public long? NumberOfDislikes { get; }

        public long? NumberOfComments { get; }

        public bool IsLiked { get; }

        public bool IsDisliked { get; }

        public bool IsLikeFlowAvailable { get; }
    }
}