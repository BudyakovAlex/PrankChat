using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Authorization
{
    public interface IAuthorizationService
    {
        Task AuthorizeAsync(string email, string password);

        Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType);

        Task<bool> AuthorizeWithAppleAsync(AppleAuthDto dto);

        Task<bool> RegisterAsync(UserRegistrationDto dto);

        Task LogoutAsync();

        Task RefreshTokenAsync();

        Task<bool?> CheckIsEmailExistsAsync(string email);

        Task<RecoverPasswordResultDto> RecoverPasswordAsync(string email);
    }
}