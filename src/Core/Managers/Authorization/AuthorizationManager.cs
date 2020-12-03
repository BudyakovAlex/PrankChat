using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Api;
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

        public Task RegisterAsync(UserRegistrationDataModel userRegistrationDataModel)
        {
            var apiModel = new UserRegistrationApiModel()
            {
                Email = userRegistrationDataModel.Email,
                Name = userRegistrationDataModel.Name,
                Login = userRegistrationDataModel.Login,
                Sex = userRegistrationDataModel.Sex,
                Birthday = userRegistrationDataModel.Birthday,
                Password = userRegistrationDataModel.Password,
                PasswordConfirmation = userRegistrationDataModel.PasswordConfirmation
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

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            var response = await _authorizationService.RecoverPasswordAsync(email);
            return response.Map();
        }

        public Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel)
        {
            var apiModel = new AppleAuthApiModel()
            {
                UserName = appleAuthDataModel.UserName,
                Email = appleAuthDataModel.Email,
                IdentityToken = appleAuthDataModel.IdentityToken,
                Token = appleAuthDataModel.Token,
                Password = appleAuthDataModel.Password,
            };

            return _authorizationService.AuthorizeWithAppleAsync(apiModel);
        }
    }
}