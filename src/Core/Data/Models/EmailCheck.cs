namespace PrankChat.Mobile.Core.Models.Data
{
    public class EmailCheck
    {
        public EmailCheck(bool result)
        {
            Result = result;
        }

        public bool Result { get; set; }
    }
}
