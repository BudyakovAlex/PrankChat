using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Authorize
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public AuthorizeService(ISettingsService settingsService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger,
                          ILogger logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<AuthorizeService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task AuthorizeAsync(string email, string password)
        {
            var loginModel = new AuthorizationApiModel { Email = email?.WithoutSpace()?.ToLower(), Password = password };
            var authTokenModel = await _client.UnauthorizedPostAsync<AuthorizationApiModel, DataApiModel<AccessTokenApiModel>>("auth/login", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            var loginTypePath = GetAuthPathByLoginType(loginType);
            var loginModel = new ExternalAuthorizationApiModel { Token = authToken };
            var authTokenModel = await _client.UnauthorizedPostAsync<ExternalAuthorizationApiModel, DataApiModel<AccessTokenApiModel>>($"auth/social/{loginTypePath}", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
            return authTokenModel?.Data?.AccessToken != null;
        }

        public async Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            var registrationApiModel = MappingConfig.Mapper.Map<UserRegistrationApiModel>(userInfo);
            var authTokenModel = await _client.UnauthorizedPostAsync<UserRegistrationApiModel, DataApiModel<AccessTokenApiModel>>("auth/register", registrationApiModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public Task LogoutAsync()
        {
            return _client.PostAsync<AuthorizationApiModel>("auth/logout", true);
        }

        public async Task RefreshTokenAsync()
        {
            var response = await _client.ExecuteRawAsync("auth/refresh", Method.POST, true);
            if (response is null)
            {
                return;
            }

            var statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.Unauthorized ||
                statusCode == HttpStatusCode.Forbidden ||
                statusCode == HttpStatusCode.InternalServerError)
            {
                _messenger.Publish(new RefreshTokenExpiredMessage(this));
            }

            var content = JsonConvert.DeserializeObject<DataApiModel<AccessTokenApiModel>>(response.Content);
            await _settingsService.SetAccessTokenAsync(content?.Data?.AccessToken);
        }

        public async Task<bool?> CheckIsEmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            var emailValidationBundle = await _client.GetAsync<EmailCheckApiModel>($"application/email/check?email={email}", true);
            return emailValidationBundle?.Result;
        }

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            var recoverPasswordModel = new RecoverPasswordApiModel { Email = email.WithoutSpace().ToLower(), };
            var result = await _client.UnauthorizedPostAsync<RecoverPasswordApiModel, RecoverPasswordResultApiModel>("auth/password/email", recoverPasswordModel, false);
            return MappingConfig.Mapper.Map<RecoverPasswordResultDataModel>(result);
        }

        public async Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel)
        {
            var authApiModel = new AppleAuthApiModel
            {
                Email = appleAuthDataModel.Email,
                IdentityToken = appleAuthDataModel.IdentityToken,
                Token = appleAuthDataModel.Token,
                UserName = appleAuthDataModel.UserName,
                Password = appleAuthDataModel.Password
            };

            var authTokenModel = await _client.UnauthorizedPostAsync<AppleAuthApiModel, DataApiModel<AccessTokenApiModel>>($"/auth/apple", authApiModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
            return authTokenModel?.Data?.AccessToken != null;
        }

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
            {
                return;
            }

            RefreshTokenAsync().FireAndForget();
        }

        private string GetAuthPathByLoginType(LoginType loginType)
        {
            switch (loginType)
            {
                case LoginType.Vk:
                    return "vk";

                case LoginType.Facebook:
                    return "fb";

                default:
                    throw new ArgumentException();
            }
        }
    }
}