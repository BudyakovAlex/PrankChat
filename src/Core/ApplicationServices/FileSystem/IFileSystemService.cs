using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.FileSystem
{
    public interface IFileSystemService
    {
        Task<bool> StoreVideoFileToGalleryAsync(string path);
    }
}
