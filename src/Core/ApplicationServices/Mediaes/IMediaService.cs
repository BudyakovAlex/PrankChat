using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Mediaes
{
    //TODO: rename to provider
    public interface IMediaService
    {
        Task<MediaFile> PickVideoAsync();

        Task<MediaFile> TakePhotoAsync();

        Task<MediaFile> PickPhotoAsync();
    }
}
