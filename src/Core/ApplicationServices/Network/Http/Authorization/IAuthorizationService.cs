﻿using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization
{
    public interface IAuthorizationService
    {
        Task AuthorizeAsync(string email, string password);

        Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType);

        Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task LogoutAsync();

        Task RefreshTokenAsync();

        Task<bool?> CheckIsEmailExistsAsync(string email);

        Task<RecoverPasswordResultApiModel> RecoverPasswordAsync(string email);
    }
}