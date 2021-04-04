using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class ArbitrationOrder
    {
        public ArbitrationOrder(
            int id,
            double? price,
            string title,
            string description,
            OrderStatusType? status,
            bool? autoProlongation,
            User customer,
            User executor,
            Video video,
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

        public double? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public bool? AutoProlongation { get; set; }

        public User Customer { get; set; }

        public User Executor { get; set; }

        public Video Video { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime? ArbitrationFinishAt { get; set; }
    }
}