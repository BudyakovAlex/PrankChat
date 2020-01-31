using System;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CreateOrderDataModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public long? Price { get; set; }

        public int? ActiveFor { get; set; }

        public bool AutoProlongation { get; set; }
    }
}
