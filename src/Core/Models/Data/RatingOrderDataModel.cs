using System;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class RatingOrderDataModel : OrderDataModel
    {
        public int Likes { get; set; }

        public int Dislikes { get; set; }
    }
}
