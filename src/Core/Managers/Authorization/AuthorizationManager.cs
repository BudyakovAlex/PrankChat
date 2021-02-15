using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
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

        public Task AuthorizeAsync(string email, string password)
        {
            return _authorizationService.AuthorizeAsync(email, password);
        }

        public Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            return _authorizationService.AuthorizeExternalAsync(authToken, loginType);
        }

        public Task RegisterAsync(UserRegistration userRegistration)
        {
            var apiModel = new UserRegistrationDto()
            {
                Email = userRegistration.Email,
                Name = userRegistration.Name,
                Login = userRegistration.Login,
                Sex = userRegistration.Sex,
                Birthday = userRegistration.Birthday,
                Password = userRegistration.Password,
                PasswordConfirmation = userRegistration.PasswordConfirmation
            };

            return _authorizationService.RegisterAsync(apiModel);
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

        public async Task<RecoverPasswordResult> RecoverPasswordAsync(string email)
        {
            var response = await _authorizationService.RecoverPasswordAsync(email);
            return response.Map();
        }

        public Task<bool> AuthorizeWithAppleAsync(AppleAuth appleAuth)
        {
            var apiModel = new AppleAuthDto
            {
                UserName = appleAuth.UserName,
                Email = appleAuth.Email,
                IdentityToken = appleAuth.IdentityToken,
                Token = appleAuth.Token,
                Password = appleAuth.Password,
            };

            return _authorizationService.AuthorizeWithAppleAsync(apiModel);
        }
    }
}