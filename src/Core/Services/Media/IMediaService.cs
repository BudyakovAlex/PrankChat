using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Media
{
    //TODO: rename to provider
    public interface IMediaService
    {
        Task<MediaFile> PickVideoAsync();

        Task<MediaFile> TakePhotoAsync();

        Task<MediaFile> PickPhotoAsync();
    }
}
