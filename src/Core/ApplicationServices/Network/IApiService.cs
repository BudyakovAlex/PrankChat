using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        #region Authorize

        Task AuthorizeAsync(string email, string password);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task LogoutAsync();

        #endregion Authorize

        #region Orders

        Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task<List<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType);

        Task<OrderDataModel> GetOrderDetailsAsync(int orderId);

        Task<OrderDataModel> TakeOrderAsync(int orderId);

        Task<List<RatingOrderDataModel>> GetRatingOrdersAsync(RatingOrderFilterType filter);

        Task CancelOrderAsync(int orderId);

        Task<OrderDataModel> SubscribeOrderAsync(int orderId);

        Task<OrderDataModel> UnsubscribeOrderAsync(int orderId);

        Task<OrderDataModel> ArgueOrderAsync(int orderId);

        Task<OrderDataModel> AcceptOrderAsync(int orderId);

        Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType value);

        #endregion Orders

        #region Publications

        Task<List<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType);

        Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null);

        Task<List<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType);

        Task<List<VideoDataModel>> GetMyVideoFeedAsync(int userId, PublicationType publicationType, DateFilterType? dateFilterType = null);

        #endregion Publications

        #region Users

        Task GetCurrentUserAsync();

        Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        Task<UserDataModel> SendAvatarAsync(string path);

        #endregion Users

        #region Video

        Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description);

        /// <summary>
        /// Registers the video viewed fact asynchronous.
        /// </summary>
        /// <param name="videoId">The video identifier.</param>
        /// <returns>Video views count.</returns>
        Task<long?> RegisterVideoViewedFactAsync(int videoId);

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
