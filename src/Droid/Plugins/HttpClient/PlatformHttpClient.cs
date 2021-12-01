using System;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Services.Network;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient
{
    public class PlatformHttpClient : IPlatformHttpClient
    {
        public Task<string> UploadVideoAsync(UploadVideoDto uploadVideoDto, Action<double, double> onChangedProgressAction = null)
        {
            throw new NotImplementedException();
        }
    }
}
