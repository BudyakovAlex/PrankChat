using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Common
{
    public interface IVersionManager
    {
        Task<AppVersion> CheckAppVersionAsync();
    }
}