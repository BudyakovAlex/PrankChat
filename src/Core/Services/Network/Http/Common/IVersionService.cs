using PrankChat.Mobile.Core.Data.Dtos;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Common
{
    public interface IVersionService
    {
        Task<AppVersionDto> CheckAppVersionAsync();
    }
}