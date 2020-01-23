using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class OrderDataModel
    {
        public int Id { get; set; }

        public long? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public DateTime? ActiveTo { get; set; }

        public bool? AutoProlongation { get; set; }

        public DateTime? CreatedAt { get; set; }

        public TimeSpan? FinishIn => DateTime.Now - ActiveTo?.ToLocalTime();

        public UserDataModel Customer { get; set; }

        public UserDataModel Executor { get; set; }
    }

}
