using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        #region Authorize

        //Task AuthorizeAsync(string email, string password);

        //Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType);

        //Task<bool> AuthorizeWithAppleAsync(AppleAuthDataModel appleAuthDataModel);

        //Task RegisterAsync(UserRegistrationDataModel userInfo);

        //Task LogoutAsync();

        //Task RefreshTokenAsync();

        //Task<bool?> CheckIsEmailExistsAsync(string email);

        //Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email);

        #endregion Authorize

        #region Orders

        //Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        //Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        //Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize);

        //Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize);

        //Task<OrderDataModel> GetOrderDetailsAsync(int orderId);

        //Task<OrderDataModel> TakeOrderAsync(int orderId);

        //Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize);

        //Task<OrderDataModel> CancelOrderAsync(int orderId);
        
        //Task ComplainOrderAsync(int orderId, string title, string description);

        //Task<OrderDataModel> SubscribeOrderAsync(int orderId);

        //Task<OrderDataModel> UnsubscribeOrderAsync(int orderId);

        //Task<OrderDataModel> ArgueOrderAsync(int orderId);

        //Task<OrderDataModel> AcceptOrderAsync(int orderId);

        //Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType value);

        #endregion Orders

        #region Publications

        //Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        //Task<VideoDataModel> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        //Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        //Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        //Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int page, int pageSize, DateFilterType? dateFilterType = null);

        #endregion Publications

        #region Users

        //Task VerifyEmailAsync();

        //Task GetCurrentUserAsync();

        //Task<PaginationModel<UserDataModel>> GetSubscriptionsAsync(int userId, int page, int pageSize);

        //Task<PaginationModel<UserDataModel>> GetSubscribersAsync(int userId, int page, int pageSize);

        //Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        //Task<UserDataModel> SendAvatarAsync(string path);
        
        //Task ComplainUserAsync(int userId, string title, string description);

        //Task<UserDataModel> GetUserAsync(int userId);

        //Task<UserDataModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null);

        //Task<UserDataModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null);

        //Task<DocumentDataModel> SendVerifyDocumentAsync(string path);

        //Task<CardDataModel> SaveCardAsync(string number, string userName);

        //Task<CardDataModel> GetCardsAsync();

        //Task DeleteCardAsync(int id);

        #endregion Users

        #region Video

        //Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description, Action<double, double> onChangedProgressAction = null, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// Registers the video viewed fact asynchronous.
        ///// </summary>
        ///// <param name="videoId">The video identifier.</param>
        ///// <returns>Video views count.</returns>
        //Task<long?> RegisterVideoViewedFactAsync(int videoId);

        //Task ComplainVideoAsync(int videoId, string title, string description);

        //Task<CommentDataModel> CommentVideoAsync(int videoId, string comment);

        //Task<PaginationModel<CommentDataModel>> GetVideoCommentsAsync(int videoId, int page, int pageSize);

        #endregion Video

        #region Payment

        //Task<PaymentDataModel> RefillAsync(double coast);

        //Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId);

        //Task<List<WithdrawalDataModel>> GetWithdrawalsAsync();

        //Task CancelWithdrawalAsync(int withdrawalId);

        #endregion Payment

        #region Notification

        //Task<PaginationModel<NotificationDataModel>> GetNotificationsAsync();

        //Task MarkNotificationsAsReadedAsync();

        //Task<int> GetUnreadNotificationsCountAsync();

        //Task SendNotificationTokenAsync(string token);

        //Task UnregisterNotificationsAsync();

        #endregion Notification

        #region Competitions

        //Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize);

        //Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize);

        //Task<List<CompetitionResultDataModel>> GetCompetitionResultsAsync(int id);

        //Task<List<CompetitionResultDataModel>> GetCompetitionRatingsAsync(int id);

        #endregion

        //Task<PaginationModel<VideoDataModel>> SearchVideosAsync(string query, int page, int pageSize);

        //Task<PaginationModel<UserDataModel>> SearchUsersAsync(string query, int page, int pageSize);

        //Task<PaginationModel<OrderDataModel>> SearchOrdersAsync(string query, int page, int pageSize);

        //Task<AppVersionDataModel> CheckAppVersionAsync();

        //Task<bool> SendLogsAsync(string filePath);
    }
}
