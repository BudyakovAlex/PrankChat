﻿using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorize
{
    public interface IAuthorizeService
    {
        Task AuthorizeAsync(string email, string password);

        Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType);

        Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task LogoutAsync();

        Task RefreshTokenAsync();

        Task<bool?> CheckIsEmailExistsAsync(string email);

        Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email);
    }
}