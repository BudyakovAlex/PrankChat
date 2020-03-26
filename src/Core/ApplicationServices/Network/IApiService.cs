using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        #region Authorize

        Task AuthorizeAsync(string email, string password);

        Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task LogoutAsync();

        Task RefreshTokenAsync();

        Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email);

        #endregion Authorize

        #region Orders

        Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        Task<OrderDataModel> GetOrderDetailsAsync(int orderId);

        Task<OrderDataModel> TakeOrderAsync(int orderId);

        Task<List<RatingOrderDataModel>> GetRatingOrdersAsync(RatingOrderFilterType filter);

        Task<OrderDataModel> CancelOrderAsync(int orderId);
        
        Task ComplainOrderAsync(int orderId, string title, string description);

        Task<OrderDataModel> SubscribeOrderAsync(int orderId);

        Task<OrderDataModel> UnsubscribeOrderAsync(int orderId);

        Task<OrderDataModel> ArgueOrderAsync(int orderId);

        Task<OrderDataModel> AcceptOrderAsync(int orderId);

        Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType value);

        #endregion Orders

        #region Publications

        Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize);

        Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int userId, PublicationType publicationType, int page, int pageSize, DateFilterType? dateFilterType = null);

        #endregion Publications

        #region Users

        Task GetCurrentUserAsync();

        Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        Task<UserDataModel> SendAvatarAsync(string path);
        
        Task ComplainUserAsync(int userId, string title, string description);

        #endregion Users

        #region Video

        Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description);

        /// <summary>
        /// Registers the video viewed fact asynchronous.
        /// </summary>
        /// <param name="videoId">The video identifier.</param>
        /// <returns>Video views count.</returns>
        Task<long?> RegisterVideoViewedFactAsync(int videoId);

        Task ComplainVideoAsync(int videoId, string title, string description);

        #endregion Video

        #region Payment

        Task<PaymentDataModel> RefillAsync(double coast);

        Task<PaymentDataModel> WithdrawalAsync(double coast);

        #endregion Payment

        #region Notification

        Task<List<NotificationDataModel>> GetNotificationsAsync();

        #endregion Notification
    }
}
