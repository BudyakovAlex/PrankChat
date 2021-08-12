using AssetsLibrary;
using Foundation;
using Microsoft.AppCenter.Crashes;
using PrankChat.Mobile.Core.Services.FileSystem;
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
				//var escapedPath = path.ToUnicode();
				_ = await library.WriteVideoToSavedPhotosAlbumAsync(NSUrl.FromFilename(path));
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