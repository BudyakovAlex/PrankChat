using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class Comment
    {
        public Comment(
            int id,
            string text,
            DateTime createdAt,
            User user)
        {
            Id = id;
            Text = text;
            CreatedAt = createdAt;
            User = user;
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}