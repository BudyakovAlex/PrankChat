using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async Task LogoutAsync()
        {
            var authTokenModel = await _client.Post<AuthorizationApiModel>("auth/logout", true);
        }

        #endregion Authorize

        #region Orders

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.Post<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder?.Data);
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
                    if (_settingsService.User == null)
                        return new List<OrderDataModel>();

                    endpoint = $"{endpoint}?executor_id={_settingsService.User.Id}";
                    break;

                case OrderFilterType.MyOwn:
                    if (_settingsService.User == null)
                        return new List<OrderDataModel>();

                    endpoint = $"{endpoint}?customer_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.Get<DataApiModel<List<OrderApiModel>>>(endpoint, includes: IncludeType.Customer);
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.Get<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint");
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<List<RatingOrderDataModel>> GetRatingOrdersAsync(RatingOrderFilterType filter)
        {
            string endpoint = "orders";
            switch (filter)
            {
                case RatingOrderFilterType.All:
                    // Nothing to do. We should use the 'orders' endpoint to get all rating orders.
                    break;

                case RatingOrderFilterType.New:
                    endpoint = $"{endpoint}?date_from={DateFilterType.Day.GetDateString()}";
                    break;

                case RatingOrderFilterType.My:
                    if (_settingsService.User == null)
                        return new List<RatingOrderDataModel>();

                    endpoint = $"{endpoint}?customer_id={_settingsService.User.Id}";
                    break;
            }
            var data = await _client.Get<DataApiModel<List<RatingOrderApiModel>>>($"{endpoint}?status={OrderStatusType.InArbitration.GetEnumMemberAttrValue()}", includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
            return MappingConfig.Mapper.Map<List<RatingOrderDataModel>>(data?.Data);
        }

        public Task CancelOrderAsync(int orderId)
        {
            return Task.CompletedTask;
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/finish", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        #endregion Orders

        #region Publications

        public async Task<VideoMetadataBundleDataModel> GetPopularVideoFeedAsync(DateFilterType dateFilterType)
        {
            VideoMetadataBundleApiModel videoMetadataBundle;
            if (_settingsService.User == null)
                videoMetadataBundle = await _client.UnauthorizedGet<VideoMetadataBundleApiModel>($"videos?popular=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);
            else
                videoMetadataBundle = await _client.Get<VideoMetadataBundleApiModel>($"videos?popular=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);

            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        public async Task<VideoMetadataBundleDataModel> GetActualVideoFeedAsync(DateFilterType dateFilterType)
        {
            VideoMetadataBundleApiModel videoMetadataBundle;
            if (_settingsService.User == null)
                videoMetadataBundle = await _client.UnauthorizedGet<VideoMetadataBundleApiModel>($"videos?actual=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);
            else
                videoMetadataBundle = await _client.Get<VideoMetadataBundleApiModel>($"videos?actual=true&date_from={dateFilterType.GetDateString()}", false, IncludeType.User);

            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        public async Task<VideoMetadataBundleDataModel> GetMyVideoFeedAsync(int userId, PublicationType publicationType, DateFilterType? dateFilterType = null)
        {
            var endpoint = "videos";
            switch (publicationType)
            {
                case PublicationType.MyVideosOfCreatedOrders:
                    endpoint += $"?customer_id={userId}";
                    break;

                case PublicationType.CompletedVideosAssignmentsByMe:
                    endpoint += $"?executor_id={userId}&status={OrderStatusType.Finished.GetEnumMemberAttrValue()}";
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            if (dateFilterType.HasValue)
                endpoint += $"&date_from={dateFilterType.Value.GetDateString()}";

            var videoMetadataBundle = await _client.Get<VideoMetadataBundleApiModel>(endpoint, false, IncludeType.User);
            return MappingConfig.Mapper.Map<VideoMetadataBundleDataModel>(videoMetadataBundle);
        }

        public async Task<VideoMetadataDataModel> SendLikeAsync(int videoId, bool isChecked)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.Post<DataApiModel<VideoMetadataApiModel>>(url);
            return MappingConfig.Mapper.Map<VideoMetadataDataModel>(data?.Data);
        }

        #endregion Publications

        #region Users

        public async Task GetCurrentUserAsync()
        {
            var dataApiModel = await _client.Get<DataApiModel<UserApiModel>>("me");
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            _settingsService.User = user;
        }

        public async Task<UserDataModel> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<UserApiModel>>("me/avatar", path);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
        }
        
        public async Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            var userUpdateProfileApiModel = MappingConfig.Mapper.Map<UserUpdateProfileApiModel>(userInfo);
            var dataApiModel = await _client.Post<UserUpdateProfileApiModel, DataApiModel<UserApiModel>>("me", userUpdateProfileApiModel);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
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
            var videoMetadataApiModel = await _client.PostVideoFile<LoadVideoApiModel, DataApiModel<VideoMetadataApiModel>>("videos", loadVideoApiModel);
            return MappingConfig.Mapper.Map<VideoMetadataDataModel>(videoMetadataApiModel.Data);
        }

        #endregion Video
    }
}
