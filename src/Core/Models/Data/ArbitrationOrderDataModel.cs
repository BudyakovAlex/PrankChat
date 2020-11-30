using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class ArbitrationOrderDataModel
    {
        public ArbitrationOrderDataModel(int id,
                                         long? price,
                                         string title,
                                         string description,
                                         OrderStatusType? status,
                                         bool? autoProlongation,
                                         UserDataModel customer,
                                         UserDataModel executor,
                                         VideoDataModel video,
                                         int likes,
                                         int dislikes,
                                         DateTime? arbitrationFinishAt)
        {
            Id = id;
            Price = price;
            Title = title;
            Description = description;
            Status = status;
            AutoProlongation = autoProlongation;
            Customer = customer;
            Executor = executor;
            Video = video;
            Likes = likes;
            Dislikes = dislikes;
            ArbitrationFinishAt = arbitrationFinishAt;
        }

        public int Id { get; set; }

        public long? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public bool? AutoProlongation { get; set; }

        public UserDataModel Customer { get; set; }

        public UserDataModel Executor { get; set; }

        public VideoDataModel Video { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime? ArbitrationFinishAt { get; set; }
    }
}
