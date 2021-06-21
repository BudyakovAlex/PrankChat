using PrankChat.Mobile.Core.ApplicationServices.FileSystem;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        public Task<bool> StoreVideoFileToGalleryAsync(string path)
        {
            //TODO: for android it is do not need
            return Task.FromResult(true);
        }
    }
}
