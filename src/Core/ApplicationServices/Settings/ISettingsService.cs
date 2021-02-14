﻿using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    //TODO: rename to provider
    public interface ISettingsService
    {
        User User { get; set; }

        string PushToken { get; set; }

        bool IsPushTokenSend { get; set; }

        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(string accessToken);

        bool IsDebugMode { get; }
    }
}
