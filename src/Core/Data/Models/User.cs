using System;
using System.ComponentModel;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class User
    {
        public User(
            int id,
            string avatar,
            string name,
            GenderType? sex,
            DateTime? birthday,
            DateTime? documentVerifiedAt,
            DateTime? emailVerifiedAt,
            Document document,
            bool isSubscribed,
            string login,
            string email,
            double? balance,
            string description,
            int? ordersOwnCount,
            int? ordersExecuteCount,
            int? ordersExecuteFinishedCount,
            int? subscribersCount,
            int? subscriptionsCount)
        {
            Id = id;
            Avatar = avatar;
            Name = name;
            Sex = sex;
            Birthday = birthday;
            DocumentVerifiedAt = documentVerifiedAt;
            EmailVerifiedAt = emailVerifiedAt;
            Document = document;
            IsSubscribed = isSubscribed;
            Login = login;
            Email = email;
            Balance = balance;
            Description = description;
            OrdersOwnCount = ordersOwnCount;
            OrdersExecuteCount = ordersExecuteCount;
            OrdersExecuteFinishedCount = ordersExecuteFinishedCount;
            SubscribersCount = subscribersCount;
            SubscriptionsCount = subscriptionsCount;
        }

        public int Id { get; set; }

        public string Avatar { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name { get; set; }

        public GenderType? Sex { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? DocumentVerifiedAt { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public Document Document { get; set; }

        public bool IsSubscribed { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public double? Balance { get; set; }

        public string Description { get; set; }

        public int? OrdersOwnCount { get; set; }

        public int? OrdersExecuteCount { get; set; }

        public int? OrdersExecuteFinishedCount { get; set; }

        public int? SubscribersCount { get; set; }

        public int? SubscriptionsCount { get; set; }
    }
}
