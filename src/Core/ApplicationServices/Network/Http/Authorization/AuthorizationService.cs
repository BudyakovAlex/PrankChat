﻿using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public AuthorizationService(ISettingsService settingsService,
                                    IMvxLogProvider logProvider,
                                    IMvxMessenger messenger,
                                    ILogger logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<AuthorizationService>();
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
            var loginModel = new AuthorizationDto { Email = email?.WithoutSpace()?.ToLower(), Password = password };
            var authTokenModel = await _client.UnauthorizedPostAsync<AuthorizationDto, ResponseDto<AccessTokenDto>>("auth/login", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            var loginTypePath = GetAuthPathByLoginType(loginType);
            var loginModel = new ExternalAuthorizationDto { Token = authToken };
            var authTokenModel = await _client.UnauthorizedPostAsync<ExternalAuthorizationDto, ResponseDto<AccessTokenDto>>($"auth/social/{loginTypePath}", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
            return authTokenModel?.Data?.AccessToken != null;
        }

        public async Task RegisterAsync(UserRegistrationDto userRegistrationApiModel)
        {
            var authTokenModel = await _client.UnauthorizedPostAsync<UserRegistrationDto, ResponseDto<AccessTokenDto>>("auth/register", userRegistrationApiModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public Task LogoutAsync()
        {
            return _client.PostAsync<AuthorizationDto>("auth/logout", true);
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

            var content = JsonConvert.DeserializeObject<ResponseDto<AccessTokenDto>>(response.Content);
            await _settingsService.SetAccessTokenAsync(content?.Data?.AccessToken);
        }

        public async Task<bool?> CheckIsEmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            var emailValidationBundle = await _client.GetAsync<EmailCheckDto>($"application/email/check?email={email}", true);
            return emailValidationBundle?.Result;
        }

        public Task<RecoverPasswordResultDto> RecoverPasswordAsync(string email)
        {
            var recoverPasswordModel = new RecoverPasswordDto { Email = email.WithoutSpace().ToLower(), };
            return _client.UnauthorizedPostAsync<RecoverPasswordDto, RecoverPasswordResultDto>("auth/password/email", recoverPasswordModel, false);
        }

        public async Task<bool> AuthorizeWithAppleAsync(AppleAuthDto appleAuthApiModel)
        {
            var authTokenModel = await _client.UnauthorizedPostAsync<AppleAuthDto, ResponseDto<AccessTokenDto>>($"/auth/apple", appleAuthApiModel, true);
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