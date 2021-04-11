using PrankChat.Mobile.Core.Providers.Platform;
using System;

namespace PrankChat.Mobile.iOS.Providers
{
    public class PlatformPathsProvider : IPlatformPathsProvider
    {
        public string DownloadsFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
