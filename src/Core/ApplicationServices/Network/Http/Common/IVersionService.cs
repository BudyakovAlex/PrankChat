using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common
{
    public interface IVersionService
    {
        Task<AppVersionApiModel> CheckAppVersionAsync();
    }
}