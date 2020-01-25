using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly HttpClient _client;

        public ApiService(ISettingsService settingsService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger)
        {
            _settingsService = settingsService;

            var log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress, configuration.ApiVersion, settingsService, log, messenger);
        }

        #region Authorize

        public async Task AuthorizeAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

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

        #endregion Authorize

        #region Orders

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.Post<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder.Data);
        }

        public async Task<List<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType)
        {
            string endpoint = "orders";
            switch (orderFilterType)
            {
                case OrderFilterType.New:
                    endpoint = $"{endpoint}?is_active=1";
                    break;

                case OrderFilterType.InProgress:
                    endpoint = $"{endpoint}?status=in_work";
                    break;

                case OrderFilterType.MyOwn:
                    if (_settingsService.User == null)
                        return new List<OrderDataModel>();

                    endpoint = $"{endpoint}?user_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.Get<DataApiModel<List<OrderApiModel>>>(endpoint, includes: IncludeType.Customer);
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.Get<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return MappingConfig.Mapper.Map<OrderDataModel>(data.Data);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint");
            return MappingConfig.Mapper.Map<OrderDataModel>(data.Data);
        }

        public async Task<List<OrderDataModel>> GetRatingOrdersAsync()
        {
            var data = await _client.Get<DataApiModel<List<RatingOrderApiModel>>>($"orders/appoint");
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        public Task CancelOrderAsync(int orderId)
        {
            return Task.CompletedTask;
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data.Data);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data.Data);
        }

        #endregion Orders

        #region Publications

        public async Task<VideoMetadataBundleDataModel> GetPopularVideoFeedAsync(DateFilterType dateFilterType)
        {
            var videoMetadataBundle =
                await _client.UnauthorizedGet<VideoMetadataBundleApiModel>($"videos?popular=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);
            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        public async Task<VideoMetadataBundleDataModel> GetActualVideoFeedAsync(DateFilterType dateFilterType)
        {
            var videoMetadataBundle = await _client.UnauthorizedGet<VideoMetadataBundleApiModel>($"videos?actual=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);
            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        public async Task<VideoMetadataBundleDataModel> GetMyVideoFeedAsync(int? userId, PublicationType publicationType, DateFilterType? dateFilterType = null)
        {
            if (userId == null)
                throw new ArgumentException("User not logged in. Please check user data.");

            var endpoint = "videos";
            switch (publicationType)
            {
                case PublicationType.MyFeedComplete:
                    endpoint += $"?user_id={userId}";
                    break;

                case PublicationType.MyFeedIncomingOrders:
                    endpoint += $"?user_id={userId}";
                    break;

                case PublicationType.MyFeedOutgoingOrders:
                    // TODO: Update endpoint for correct filter parameters
                    endpoint += $"?user_id={userId}";
                    break;

                default:
                    // TODO: Update endpoint for correct filter parameters
                    endpoint += $"?user_id={userId}";
                    break;
            }

            if (dateFilterType.HasValue)
                endpoint += $"&date_from={dateFilterType.Value.GetDateString()}";

            var videoMetadataBundle = await _client.UnauthorizedGet<VideoMetadataBundleApiModel>(endpoint, false, IncludeType.User);
            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        #endregion Publications

        #region Users

        public async Task GetCurrentUser()
        {
            var dataApiModel = await _client.Get<DataApiModel<UserApiModel>>("me");
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel.Data);
            _settingsService.User = user;
        }

        #endregion Users

        #region Video

        public async Task<VideoMetadataDataModel> SendVideoAsync(int orderId, string path, string title, string description)
        {
            var loadVideoApiModel = new LoadVideoApiModel()
            {
                OrderId = orderId,
                FilePath = path,
                Title = title,
                Description = description,
            };
            var videoMetadataApiModel = await _client.PostFile<LoadVideoApiModel, DataApiModel<VideoMetadataApiModel>>("videos", loadVideoApiModel);
            return MappingConfig.Mapper.Map<VideoMetadataDataModel>(videoMetadataApiModel.Data);
        }

        #endregion Video
    }
}
