using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class RatingOrderDataModel
    {
        public int Id { get; set; }

        public long? Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatusType? Status { get; set; }

        public bool? AutoProlongation { get; set; }

        public UserDataModel Customer { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime? ArbitrationFinishAt { get; set; }
    }
}
