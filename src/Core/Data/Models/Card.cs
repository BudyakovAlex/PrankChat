namespace PrankChat.Mobile.Core.Models.Data
{
    public class Card
    {
        public Card(
            int id,
            string number,
            string cardUserName)
        {
            Id = id;
            Number = number;
            CardUserName = cardUserName;
        }

        public int Id { get; set; }

        public string Number { get; set; }

        public string CardUserName { get; set; }
    }
}