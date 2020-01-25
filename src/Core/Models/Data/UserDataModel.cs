﻿namespace PrankChat.Mobile.Core.Models.Data
{
    public class UserDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public double? Balance { get; set; }

        public string Avatar { get; set; }

        public int? OrdersOwnCount { get; set; }

        public int? OrdersExecuteCount { get; set; }

        public int? OrdersExecuteFinishedCount { get; set; }

        public int? SubscribersCount { get; set; }

        public int? SubscriptionsCount { get; set; }
    }
}