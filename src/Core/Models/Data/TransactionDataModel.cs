namespace PrankChat.Mobile.Core.Models.Data
{
    public class TransactionDataModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Comment { get; set; }

        public string Direction { get; set; }

        public string Reason { get; set; }

        public double? BalanceBefore { get; set; }

        public double? BalanceAfter { get; set; }

        public double? FrozenBefore { get; set; }

        public double? FrozenAfter { get; set; }

        public UserDataModel User { get; set; }
    }
}
