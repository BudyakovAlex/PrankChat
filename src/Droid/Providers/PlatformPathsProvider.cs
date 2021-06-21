using PrankChat.Mobile.Core.Providers.Platform;
using System.IO;

namespace PrankChat.Mobile.Droid.Providers
{
    public class PlatformPathsProvider : IPlatformPathsProvider
    {
        public string DownloadsFolderPath => GetDownloadsFolder();

        private string GetDownloadsFolder()
        {
            var downloadDirectory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            return downloadDirectory;
        }
    }
}