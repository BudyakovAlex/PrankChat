﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Plugin.DeviceInfo;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public ApiService(ISettingsService settingsService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger,
                          IDialogService dialogService)
        {
            _settingsService = settingsService;
            _messenger = messenger;

            _log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     messenger,
                                     dialogService);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        #region Authorize

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
            var authTokenModel = await _client.PostAsync<DataApiModel<AccessTokenApiModel>>("auth/refresh", true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            var recoverPasswordModel = new RecoverPasswordApiModel { Email = email.WithoutSpace().ToLower(), };
            var result = await _client.UnauthorizedPostAsync<RecoverPasswordApiModel, RecoverPasswordResultApiModel>("auth/password/email", recoverPasswordModel, false);
            return MappingConfig.Mapper.Map<RecoverPasswordResultDataModel>(result);
        }

        #endregion Authorize

        #region Orders

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.PostAsync<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder?.Data);
        }

        public async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var endpoint = $"{orderFilterType.GetUrlResource()}?page={page}&items_per_page={pageSize}";
            switch (orderFilterType)
            {
                case OrderFilterType.MyOwn when _settingsService.User != null:
                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;

                case OrderFilterType.MyOwn when _settingsService.User == null:
                case OrderFilterType.MyCompletion when _settingsService.User == null:
                case OrderFilterType.MyOrdered when _settingsService.User == null:
                    return new PaginationModel<OrderDataModel>();
            }

            var data = await _client.GetAsync<BaseBundleApiModel<OrderApiModel>>(endpoint, includes: IncludeType.Customer);
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.GetAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            var endpoint = $"orders?page={page}&items_per_page={pageSize}&status={OrderStatusType.InArbitration.GetEnumMemberAttrValue()}";
            switch (filter)
            {
                case ArbitrationOrderFilterType.All:
                    // Nothing to do. We should use the 'orders' endpoint to get all rating orders.
                    break;

                case ArbitrationOrderFilterType.New:
                    endpoint = $"{endpoint}&date_from={DateFilterType.Day.GetDateString()}";
                    break;

                case ArbitrationOrderFilterType.My:
                    if (_settingsService.User == null)
                        return new PaginationModel<ArbitrationOrderDataModel>(new List<ArbitrationOrderDataModel>());

                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.GetAsync<BaseBundleApiModel<ArbitrationOrderApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
            return CreatePaginationResult<ArbitrationOrderApiModel, ArbitrationOrderDataModel>(data);
        }

        public async Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/cancel", false);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"orders/{orderId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/finish", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var arbitrationValue = new ChangeArbitrationApiModel()
            {
                Value = isLiked.ToString().ToLower(),
            };
            var data = await _client.PostAsync<ChangeArbitrationApiModel, DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration/value", arbitrationValue, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        #endregion Orders

        #region Publications

        public async Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/popular?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            return CreatePaginationResult<VideoApiModel, VideoDataModel>(videoMetadataBundle);
        }

        public async Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            var endpoint = $"newsline/videos/new?period={dateFilterType.GetEnumMemberAttrValue()}&page={page}&items_per_page={pageSize}";
            var videoMetadataBundle = _settingsService.User == null ?
                await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer) :
                await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);

            var mappedModels = MappingConfig.Mapper.Map<List<VideoDataModel>>(videoMetadataBundle?.Data);
            var paginationData = videoMetadataBundle.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? mappedModels.Count;

            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public async Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int userId, PublicationType publicationType, int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            if (_settingsService.User == null)
            {
                return new PaginationModel<VideoDataModel>();
            }

            var endpoint = string.Empty;
            switch (publicationType)
            {
                case PublicationType.MyVideosOfCreatedOrders:
                    endpoint = $"videos?page={page}&items_per_page={pageSize}&user_id={userId}";
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            if (dateFilterType.HasValue)
                endpoint += $"&date_from={dateFilterType.Value.GetDateString()}";

            var dataApiModel = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.Customer);
            var orderDataModel = MappingConfig.Mapper.Map<List<VideoDataModel>>(dataApiModel?.Data);
            var paginationData = dataApiModel.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? orderDataModel.Count;
            return new PaginationModel<VideoDataModel>(orderDataModel, totalItemsCount);
        }

        public async Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.PostAsync<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return MappingConfig.Mapper.Map<VideoDataModel>(data?.Data);
        }

        #endregion Publications

        #region Users

        public Task VerifyEmailAsync()
        {
            return _client.PostAsync<DataApiModel>("me/verify/resend");
        }

        public async Task GetCurrentUserAsync()
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<UserApiModel>>("me", includes: IncludeType.Document);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            _settingsService.User = user;
        }

        public async Task<UserDataModel> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<UserApiModel>>("me/picture", path, "avatar");
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            var userUpdateProfileApiModel = MappingConfig.Mapper.Map<UserUpdateProfileApiModel>(userInfo);
            var dataApiModel = await _client.PostAsync<UserUpdateProfileApiModel, DataApiModel<UserApiModel>>("me", userUpdateProfileApiModel);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"users/{userId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<DocumentDataModel> SendVerifyDocumentAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<DocumentApiModel>>("user/dcs", path, "document");
            var user = MappingConfig.Mapper.Map<DocumentDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<CardDataModel> SaveCardAsync(string number, string userName)
        {
            var createCreditCard = new CreateCardApiModel()
            {
                Number = number.WithoutSpace(),
                CardUserName = userName,
            };
            var dataApiModel = await _client.PostAsync<CreateCardApiModel, DataApiModel<CardApiModel>>("me/cards", createCreditCard, true);
            var user = MappingConfig.Mapper.Map<CardDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<CardDataModel> GetCardsAsync()
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<List<CardApiModel>>>("me/cards");
            var data = MappingConfig.Mapper.Map<List<CardDataModel>>(dataApiModel?.Data);
            return data?.FirstOrDefault();
        }

        public Task DeleteCardAsync(int id)
        {
            return _client.DeleteAsync($"me/cards/{id}", true);
        }

        #endregion Users

        #region Video

        public async Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description, Action<double> onChangedProgressAction = null)
        {
            var loadVideoApiModel = new LoadVideoApiModel()
            {
                OrderId = orderId,
                FilePath = path,
                Title = title,
                Description = description,
            };
            var videoMetadataApiModel = await _client.PostVideoFileAsync<LoadVideoApiModel, DataApiModel<VideoApiModel>>("videos", loadVideoApiModel, onChangedProgressAction: onChangedProgressAction);
            return MappingConfig.Mapper.Map<VideoDataModel>(videoMetadataApiModel?.Data);
        }

        public async Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            var videoApiModel = await _client.UnauthorizedGetAsync<DataApiModel<VideoApiModel>>($"videos/{videoId}/looked");
            _log.Log(MvxLogLevel.Debug, () => $"Registered {videoApiModel?.Data?.ViewsCount} for video with id {videoId}");
            return videoApiModel?.Data?.ViewsCount;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"videos/{videoId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<CommentDataModel> CommentVideoAsync(int videoId, string comment)
        {
            var dataApiModel = new SendCommentApiModel
            {
                Text = comment,
            };

            var url = $"videos/{videoId}/comments";
            var dataModel = await _client.PostAsync<SendCommentApiModel, DataApiModel<CommentApiModel>>(url, dataApiModel);
            return MappingConfig.Mapper.Map<CommentDataModel>(dataModel?.Data);
        }

        public async Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            var data = await _client.GetAsync<BaseBundleApiModel<CommentApiModel>>($"videos/{videoId}/comments?page={page}&items_per_page={pageSize}");
            return CreatePaginationResult<CommentApiModel, CommentDataModel>(data);
        }

        #endregion Video

        #region Payment

        public async Task<PaymentDataModel> RefillAsync(double coast)
        {
            var refillApiData = new RefillApiData
            {
                Amount = coast
            };

            var data = await _client.PostAsync<RefillApiData, DataApiModel<PaymentApiModel>>("payment", refillApiData);
            return MappingConfig.Mapper.Map<PaymentDataModel>(data?.Data);
        }

        public async Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId)
        {
            var createWithdrawalApiModel = new CreateWithdrawalApiModel()
            {
                Amount = coast,
                CreditCardId = cardId,
            };

            var dataApiModel = await _client.PostAsync<CreateWithdrawalApiModel, DataApiModel<WithdrawalApiModel>>("withdrawal", createWithdrawalApiModel);
            var data = MappingConfig.Mapper.Map<WithdrawalDataModel>(dataApiModel?.Data);
            return data;
        }

        public async Task<List<WithdrawalDataModel>> GetWithdrawalsAsync()
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<List<WithdrawalApiModel>>> ("withdrawal");
            var data = MappingConfig.Mapper.Map<List<WithdrawalDataModel>>(dataApiModel?.Data);
            return data;
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _client.DeleteAsync($"withdrawal/{withdrawalId}", true);
        }

        #endregion Payment

        #region Notification

        public async Task<List<NotificationDataModel>> GetNotificationsAsync()
        {
            var notificationBundle = await _client.GetAsync<BaseBundleApiModel<NotificationApiModel>>("notifications");
            return MappingConfig.Mapper.Map<List<NotificationDataModel>>(notificationBundle?.Data);
        }

        public Task SendNotificationTokenAsync(string token)
        {
            var pushNotificationApiMode = new PushNotificationApiMode()
            {
                Token = token,
                DeviceId = CrossDeviceInfo.Current.Id,
            };
            return _client.PostAsync("me/device", pushNotificationApiMode, true);
        }

        #endregion Notification

        #region Competitions

        public async Task<List<CompetitionResultDataModel>> GetCompetitionResultsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleApiModel<CompetitionResultApiModel>>($"competition/{id}/results");
            return MappingConfig.Mapper.Map<List<CompetitionResultDataModel>>(bundle?.Data);
        }

        public async Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id)
        {
            var bundle = await _client.GetAsync<BaseBundleApiModel<CompetitionResultApiModel>>($"competition/{id}/rating");
            return MappingConfig.Mapper.Map<List<CompetitionResultDataModel>>(bundle?.Data);
        }

        public async Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            BaseBundleApiModel<VideoApiModel> videoMetadataBundle;
            if (_settingsService.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGetAsync<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.GetAsync<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            return CreatePaginationResult<VideoApiModel, VideoDataModel>(videoMetadataBundle);
        }

        public async Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            var endpoint = $"competitions?page={page}&items_per_page={pageSize}";
            var data = await _client.GetAsync<BaseBundleApiModel<CompetitionApiModel>>(endpoint);
            return CreatePaginationResult<CompetitionApiModel, CompetitionDataModel>(data);
        }

        #endregion

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
                return;

            RefreshTokenAsync().FireAndForget();
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data ?? new List<TApiModel>());
            var paginationData = data?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}
