using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CommentDataModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserDataModel User { get; set; }
    }
}