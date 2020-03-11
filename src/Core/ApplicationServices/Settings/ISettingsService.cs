﻿using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public interface ISettingsService
    {
        UserDataModel User { get; set; }

        string PushToken { get; set; }

        bool IsPushnTokenSend { get; set; }

        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(string accessToken);
    }
}
