using PrankChat.Mobile.Core.Models.Enums;
using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class VideoDataModel
    {
        public VideoDataModel(int id,
                              string title,
                              string description,
                              string poster,
                              string status,
                              long? viewsCount,
                              long? repostsCount,
                              long? likesCount,
                              long? dislikesCount,
                              long? commentsCount,
                              string streamUri,
                              string previewUri,
                              string markedStreamUri,
                              string shareUri,
                              bool isLiked,
                              bool isDisliked,
                              OrderCategory? orderCategory,
                              DateTimeOffset createdAt,
                              UserDataModel user,
                              UserDataModel customer)
        {
            Id = id;
            Title = title;
            Description = description;
            Poster = poster;
            Status = status;
            ViewsCount = viewsCount;
            RepostsCount = repostsCount;
            LikesCount = likesCount;
            DislikesCount = dislikesCount;
            CommentsCount = commentsCount;
            StreamUri = streamUri;
            PreviewUri = previewUri;
            MarkedStreamUri = markedStreamUri;
            ShareUri = shareUri;
            IsLiked = isLiked;
            IsDisliked = isDisliked;
            OrderCategory = orderCategory;
            CreatedAt = createdAt;
            User = user;
            Customer = customer;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Poster { get; set; }

        public string Status { get; set; }

        public long? ViewsCount { get; set; }

        public long? RepostsCount { get; set; }

        public long? LikesCount { get; set; }

        public long? DislikesCount { get; set; }

        public long? CommentsCount { get; set; }

        public string StreamUri { get; set; }

        public string PreviewUri { get; set; }

        public string MarkedStreamUri { get; set; }

        public string ShareUri { get; set; }

        public bool IsLiked { get; set; }

        public bool IsDisliked { get; set; }

        public OrderCategory? OrderCategory { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public UserDataModel User { get; set; }

        public UserDataModel Customer { get; set; }
    }
}
