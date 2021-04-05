namespace PrankChat.Mobile.Core.Models.Data
{
    public class Document
    {
        public Document(int id, string path)
        {
            Id = id;
            Path = path;
        }

        public int Id { get; set; }

        public string Path { get; set; }
    }
}
