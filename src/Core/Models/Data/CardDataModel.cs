using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class CardDataModel
    {
        public CardDataModel(int id, string number, string cardUserName)
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
