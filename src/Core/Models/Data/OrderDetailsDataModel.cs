using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class OrderDetailsDataModel
    {
        public string Id { get; set; }

        public double Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType Status { get; set; }

        public DateTime ActiveTo { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserDataModel Сustomer { get; set; }

        public UserDataModel Executor { get; set; }
    }
}
