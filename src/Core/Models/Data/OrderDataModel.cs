using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class OrderDataModel
    {
        public OrderDataModel(int id,
                              double? price,
                              string title,
                              string description,
                              OrderStatusType? status,
                              OrderCategory? orderCategory,
                              DateTime? activeTo,
                              int durationInHours,
                              bool? autoProlongation,
                              DateTime? createdAt,
                              DateTime? takenToWorkAt,
                              DateTime? videoUploadedAt,
                              DateTime? arbitrationFinishAt,
                              DateTime? closeOrderAt,
                              UserDataModel customer,
                              UserDataModel executor,
                              VideoDataModel video,
                              ArbitrationValueType? myArbitrationValue,
                              int? negativeArbitrationValuesCount,
                              int? positiveArbitrationValuesCount)
        {
            Id = id;
            Price = price;
            Title = title;
            Description = description;
            Status = status;
            OrderCategory = orderCategory;
            ActiveTo = activeTo;
            DurationInHours = durationInHours;
            AutoProlongation = autoProlongation;
            CreatedAt = createdAt;
            TakenToWorkAt = takenToWorkAt;
            VideoUploadedAt = videoUploadedAt;
            ArbitrationFinishAt = arbitrationFinishAt;
            CloseOrderAt = closeOrderAt;
            Customer = customer;
            Executor = executor;
            Video = video;
            MyArbitrationValue = myArbitrationValue;
            NegativeArbitrationValuesCount = negativeArbitrationValuesCount;
            PositiveArbitrationValuesCount = positiveArbitrationValuesCount;
        }

        public int Id { get; set; }

        public double? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public OrderCategory? OrderCategory { get; set; }

        public DateTime? ActiveTo { get; set; }

        public int DurationInHours { get; set; }

        public bool? AutoProlongation { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? TakenToWorkAt { get; set; }

        public DateTime? VideoUploadedAt { get; set; }

        public DateTime? ArbitrationFinishAt { get; set; }

        public DateTime? CloseOrderAt { get; set; }

        public UserDataModel Customer { get; set; }

        public UserDataModel Executor { get; set; }

        public VideoDataModel Video { get; set; }

        public ArbitrationValueType? MyArbitrationValue { get; set; }

        public int? NegativeArbitrationValuesCount { get; set; }

        public int? PositiveArbitrationValuesCount { get; set; }

        public TimeSpan? CloseOrderIn => CloseOrderAt?.ToLocalTime() - DateTime.Now;

        public TimeSpan? FinishIn => ActiveTo?.ToLocalTime() - DateTime.Now;

        public TimeSpan? ArbitrationFinishIn => ArbitrationFinishAt?.ToLocalTime() - DateTime.Now;

        public TimeSpan? VideoUploadedIn => ActiveTo?.ToLocalTime() - VideoUploadedAt?.ToLocalTime();
    }
}
