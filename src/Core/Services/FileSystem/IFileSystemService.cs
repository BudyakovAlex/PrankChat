using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.FileSystem
{
    public interface IFileSystemService
    {
        Task<bool> StoreVideoFileToGalleryAsync(string path);
    }
}
