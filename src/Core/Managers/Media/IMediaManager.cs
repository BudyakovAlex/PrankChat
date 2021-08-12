using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Media
{
    //TODO: rename to provider
    public interface IMediaManager
    {
        Task<MediaFile> PickVideoAsync();

        Task<MediaFile> TakePhotoAsync();

        Task<MediaFile> PickPhotoAsync();
    }
}
