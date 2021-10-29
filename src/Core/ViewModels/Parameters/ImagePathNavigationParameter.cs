namespace PrankChat.Mobile.Core.ViewModels.Parameters
{
    public class ImagePathNavigationParameter
    {
        public ImagePathNavigationParameter(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
