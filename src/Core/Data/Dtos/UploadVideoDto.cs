namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class UploadVideoDto
    {
        public int OrderId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Video { get; set; }

        public string FilePath { get; set; }
    }
}