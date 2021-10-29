using PrankChat.Mobile.Core.Providers.Platform;
using System;

namespace PrankChat.Mobile.iOS.Common
{
    public class PlatformPathsProvider : IPlatformPathsProvider
    {
        public string DownloadsFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
