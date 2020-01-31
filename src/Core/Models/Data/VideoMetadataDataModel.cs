using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class VideoMetadataDataModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public long ViewsCount { get; set; }

        public long RepostsCount { get; set; }

        public string StreamUri { get; set; }

        public string ShareUri { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public UserDataModel User { get; set; }
    }
}
