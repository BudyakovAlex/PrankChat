namespace PrankChat.Mobile.Core.Models.Data
{
    public class EmailCheckDataModel
    {
        public EmailCheckDataModel(bool result)
        {
            Result = result;
        }

        public bool Result { get; set; }
    }
}
