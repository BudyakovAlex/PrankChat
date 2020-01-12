﻿using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        Task AuthorizeAsync(string email, string password);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task GetOrdersAsync();
    }
}
