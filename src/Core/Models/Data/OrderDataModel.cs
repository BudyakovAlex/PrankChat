using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class OrderDataModel
    {
        public int Id { get; set; }

        public double? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public DateTime? ActiveTo { get; set; }

        public bool? AutoProlongation { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? TakenToWorkAt { get; set; }

        public TimeSpan? FinishIn => ActiveTo?.ToLocalTime() - DateTime.Now;

        public UserDataModel Customer { get; set; }

        public UserDataModel Executor { get; set; }

        public VideoDataModel Video { get; set; }

        public ArbitrationValueType? MyArbitrationValue { get; set; }

        public int? NegativeArbitrationValuesCount { get; set; }

        public int? PositiveArbitrationValuesCount { get; set; }
    }
}
