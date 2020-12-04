namespace PrankChat.Mobile.Core.Models.Data
{
    public class RecoverPasswordResultDataModel
    {
        public RecoverPasswordResultDataModel(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
    }
}
