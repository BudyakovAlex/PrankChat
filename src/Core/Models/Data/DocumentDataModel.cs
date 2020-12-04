namespace PrankChat.Mobile.Core.Models.Data
{
    public class DocumentDataModel
    {
        public DocumentDataModel(int id, string path)
        {
            Id = id;
            Path = path;
        }

        public int Id { get; set; }

        public string Path { get; set; }
    }
}
