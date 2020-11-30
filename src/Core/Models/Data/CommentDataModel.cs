using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CommentDataModel
    {
        public CommentDataModel(int id,
                                string text,
                                DateTime createdAt,
                                UserDataModel user)
        {
            Id = id;
            Text = text;
            CreatedAt = createdAt;
            User = user;
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserDataModel User { get; set; }
    }
}