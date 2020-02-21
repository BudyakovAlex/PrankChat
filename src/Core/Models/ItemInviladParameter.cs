namespace PrankChat.Mobile.Core.Models
{
    public class ItemInvalidParameter
    {
        public string Name { get; set; }

        public string Reason { get; set; }

        public override string ToString() => $"{Name}: {Reason}";
    }
}
