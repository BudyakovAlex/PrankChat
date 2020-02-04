namespace PrankChat.Mobile.Core.Models.Data
{
    public class TransactionDataModel
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public string Comment { get; set; }

        public string Direction { get; set; }

        public int BalanceBefore { get; set; }

        public double BalanceAfter { get; set; }

        public int FrozenBefore { get; set; }

        public int FrozenAfter { get; set; }
    }
}
