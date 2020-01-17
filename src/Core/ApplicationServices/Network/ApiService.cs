﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Storages;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly HttpClient _client;
        private readonly IStorageService _storageService;

        public ApiService(ISettingsService settingsService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger,
                          IStorageService storageService)
        {
            _settingsService = settingsService;
            _storageService = storageService;

            var log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress, configuration.ApiVersion, settingsService, log, messenger);
        }

        #region Authorize 

        public async Task AuthorizeAsync(string email, string password)
        {
            var loginModel = new AuthorizationApiModel { Email = email, Password = password };
            var authTokenModel = await _client.UnauthorizedPost<AuthorizationApiModel, DataApiModel<AccessTokenApiModel>>("auth/login", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            var registrationApiModel = MappingConfig.Mapper.Map<UserRegistrationApiModel>(userInfo);
            var authTokenModel = await _client.UnauthorizedPost<UserRegistrationApiModel, DataApiModel<AccessTokenApiModel>>("auth/register", registrationApiModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        #endregion

        #region Orders

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.Post<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder.Data);
        }

        public async Task<List<OrderDataModel>> GetOrdersAsync()
        {
            var data = await _client.Get<DataApiModel<List<OrderApiModel>>>("orders");
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        public async Task<OrderDetailsDataModel> GetOrderDetailsAsync(string orderId)
        {
            var data = await _client.Get<DataApiModel<OrderDetailsApiModel>>($"orders/{orderId}");
            return MappingConfig.Mapper.Map<OrderDetailsDataModel>(data.Data);
        }

        public async Task<OrderDetailsDataModel> TakeOrderAsync(string orderId)
        {
            var data = await _client.Post<DataApiModel<OrderDetailsApiModel>>($"orders/{orderId}/executor​/appoint");
            return MappingConfig.Mapper.Map<OrderDetailsDataModel>(data.Data);
        }

        public async Task<List<OrderDataModel>> GetRatingOrdersAsync()
        {
            var data = await _client.Get<DataApiModel<List<OrderDataModel>>>($"orders/appoint");
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        #endregion

        #region Publications

        public async Task<VideoMetadataBundleDataModel> GetVideoFeedAsync()
        {
            var videoMetadataBundle = await _client.UnauthorizedGet<VideoMetadataBundleApiModel>("videos");
            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        #endregion

        #region Users

        public async Task GetCurrentUser()
        {
            var dataApiModel = await _client.Get<DataApiModel<UserApiModel>>("me");
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel.Data);
            _storageService.User = user;
        }

        #endregion
    }
}
