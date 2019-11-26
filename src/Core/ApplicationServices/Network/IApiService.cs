using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        Task AuthorizeAsync(string email, string password);
    }
}
