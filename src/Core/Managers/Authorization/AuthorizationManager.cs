using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Services.Analytics;
using PrankChat.Mobile.Core.Services.Network.Http.Authorization;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Authorization
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAnalyticsService _analyticsService;

        public AuthorizationManager(IAuthorizationService authorizationService, IAnalyticsService analyticsService)
        {
            _authorizationService = authorizationService;
            _analyticsService = analyticsService;
        }

        public Task AuthorizeAsync(string email, string password)
        {
            return _authorizationService.AuthorizeAsync(email, password);
        }

        public Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            return _authorizationService.AuthorizeExternalAsync(authToken, loginType);
        }

        public async Task RegisterAsync(UserRegistration userRegistration)
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

            var isRegistered = await _authorizationService.RegisterAsync(apiModel);
            if (isRegistered)
            {
                _analyticsService.Track(AnalyticsEvent.RegistrationManual);
            }
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