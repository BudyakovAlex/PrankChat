using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Managers.Authorization
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationManager(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public Task AuthorizeAsync(string email, string password)
        {
            return _authorizationService.AuthorizeAsync(email, password);
        }

        public Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            return _authorizationService.AuthorizeExternalAsync(authToken, loginType);
        }

        public Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            return _authorizationService.RegisterAsync(userInfo);
        }

        public Task LogoutAsync()
        {
            return _authorizationService.LogoutAsync();
        }

        public Task RefreshTokenAsync()
        {
            return _authorizationService.RefreshTokenAsync();
        }

        public Task<bool?> CheckIsEmailExistsAsync(string email)
        {
            return _authorizationService.CheckIsEmailExistsAsync(email);
        }

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            var response = await _authorizationService.RecoverPasswordAsync(email);
            return response.Map();
        }

        public Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel)
        {
            return _authorizationService.AuthorizeWithAppleAsync(appleAuthDataModel);
        }
    }
}