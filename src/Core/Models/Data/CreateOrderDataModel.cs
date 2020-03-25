using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CreateOrderDataModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double? Price { get; set; }

        public int? ActiveFor { get; set; }

        public bool AutoProlongation { get; set; }
    }
}
