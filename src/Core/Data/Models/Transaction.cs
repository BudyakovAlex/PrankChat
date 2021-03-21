namespace PrankChat.Mobile.Core.Models.Data
{
    public class Transaction
    {
        public Transaction(
            int id,
            double? amount,
            string comment,
            string direction,
            string reason,
            double? balanceBefore,
            double? balanceAfter,
            double? frozenBefore,
            double? frozenAfter,
            User user)
        {
            Id = id;
            Amount = amount;
            Comment = comment;
            Direction = direction;
            Reason = reason;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            FrozenBefore = frozenBefore;
            FrozenAfter = frozenAfter;
            User = user;
        }

        public int Id { get; set; }

        public double? Amount { get; set; }

        public string Comment { get; set; }

        public string Direction { get; set; }

        public string Reason { get; set; }

        public double? BalanceBefore { get; set; }

        public double? BalanceAfter { get; set; }

        public double? FrozenBefore { get; set; }

        public double? FrozenAfter { get; set; }

        public User User { get; set; }
    }
}
