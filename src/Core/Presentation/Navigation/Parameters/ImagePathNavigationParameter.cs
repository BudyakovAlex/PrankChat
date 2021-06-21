namespace PrankChat.Mobile.Core.Presentation.Navigation.Parameters
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
