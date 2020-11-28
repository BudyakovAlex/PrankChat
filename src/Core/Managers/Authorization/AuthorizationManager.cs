using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Authorization
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationManager(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task AuthorizeAsync(string email, string password)
        {
            await _authorizationService.AuthorizeAsync(email, password);
        }

        public async Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            return await _authorizationService.AuthorizeExternalAsync(authToken, loginType);
        }

        public async Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            await _authorizationService.RegisterAsync(userInfo);
        }

        public Task LogoutAsync()
        {
            return _authorizationService.LogoutAsync();
        }

        public async Task RefreshTokenAsync()
        {
            await _authorizationService.RefreshTokenAsync();
        }

        public async Task<bool?> CheckIsEmailExistsAsync(string email)
        {
            return await _authorizationService.CheckIsEmailExistsAsync(email);
        }

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            return await _authorizationService.RecoverPasswordAsync(email);
        }

        public async Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel)
        {
            return await _authorizationService.AuthorizeWithAppleAsync(appleAuthDataModel);
        }
    }
}