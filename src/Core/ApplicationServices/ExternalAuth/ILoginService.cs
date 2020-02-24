using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.ExternalAuth
{
    interface ILoginService
    {
        Task<LoginResult> Login();
        void Logout();
    }
}
