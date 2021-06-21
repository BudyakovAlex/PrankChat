namespace PrankChat.Mobile.Core.Models.Data
{
    public class Payment
    {
        public Payment(
            int id,
            double? amount,
            string provider,
            string status,
            string paymentLink)
        {
            Id = id;
            Amount = amount;
            Provider = provider;
            Status = status;
            PaymentLink = paymentLink;
        }

        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Provider { get; set; }

        public string Status { get; set; }

        public string PaymentLink { get; set; }
    }
}
