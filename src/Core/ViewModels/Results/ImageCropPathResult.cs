namespace PrankChat.Mobile.Core.ViewModels.Results
{
    public class ImageCropPathResult
    {
        public ImageCropPathResult(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
