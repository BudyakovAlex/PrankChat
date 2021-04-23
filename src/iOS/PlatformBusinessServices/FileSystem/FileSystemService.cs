using AssetsLibrary;
using Foundation;
using Microsoft.AppCenter.Crashes;
using PrankChat.Mobile.Core.ApplicationServices.FileSystem;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.iOS.PlatformBusinessServices.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        public async Task<bool> StoreVideoFileToGalleryAsync(string path)
        {
			try
			{
				var library = new ALAssetsLibrary();
				_ = await library.WriteVideoToSavedPhotosAlbumAsync(new NSUrl(path));
				return true;
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
				return false;
			}
		}
    }
}