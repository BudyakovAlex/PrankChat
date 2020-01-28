using System;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace PrankChat.Mobile.Core.ApplicationServices.Mediaes
{
    public interface IMediaService
    {
        Task<MediaFile> PickVideoAsync();

        Task<MediaFile> TakePhotoAsync();

        Task<MediaFile> PickPhotoAsync();
    }
}
