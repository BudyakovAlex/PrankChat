namespace PrankChat.Mobile.Core.Models.Data
{
    public class RecoverPasswordResult
    {
        public RecoverPasswordResult(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
    }
}
