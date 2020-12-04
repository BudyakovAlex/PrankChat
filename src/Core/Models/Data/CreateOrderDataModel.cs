namespace PrankChat.Mobile.Core.Models.Data
{
    public class CreateOrderDataModel
    {
        public CreateOrderDataModel(string title,
                                    string description,
                                    double price,
                                    int activeFor,
                                    bool autoProlongation,
                                    bool isHidden)
        {
            Title = title;
            Description = description;
            Price = price;
            ActiveFor = activeFor;
            AutoProlongation = autoProlongation;
            IsHidden = isHidden;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int ActiveFor { get; set; }

        public bool AutoProlongation { get; set; }

        public bool IsHidden { get; set; }
    }
}