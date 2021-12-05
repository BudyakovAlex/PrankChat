using System;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Data.Dtos;

namespace PrankChat.Mobile.Core.Services.Network
{
    public interface IPlatformHttpClient
    {
        Task<string> UploadVideoAsync(UploadVideoDto uploadVideoDto, Action<double, double> onChangedProgressAction = null);
    }
}
