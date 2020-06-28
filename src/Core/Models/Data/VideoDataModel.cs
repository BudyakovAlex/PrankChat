using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class VideoDataModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Poster { get; set; }

        public string Status { get; set; }

        public long ViewsCount { get; set; }

        public long RepostsCount { get; set; }

        public long LikesCount { get; set; }

        public long DislikesCount { get; set; }

        public long CommentsCount { get; set; }

        public string StreamUri { get; set; }

        public string PreviewUri { get; set; }

        public string MarkedStreamUri { get; set; }

        public string ShareUri { get; set; }

        public bool IsLiked { get; set; }

        public bool IsDisliked { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public UserDataModel User { get; set; }

        public UserDataModel Customer { get; set; }
    }
}
