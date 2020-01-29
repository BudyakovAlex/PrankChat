using System.Collections.Generic;
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

        Task<List<OrderDataModel>> GetRatingOrdersAsync();

        Task CancelOrderAsync(int orderId);

        Task<OrderDataModel> SubscribeOrderAsync(int orderId);

        Task<OrderDataModel> UnsubscribeOrderAsync(int orderId);

        #endregion Orders

        #region Publications

        Task<VideoMetadataBundleDataModel> GetPopularVideoFeedAsync(DateFilterType dateFilterType);

        Task<VideoMetadataBundleDataModel> GetActualVideoFeedAsync(DateFilterType dateFilterType);

        Task<VideoMetadataBundleDataModel> GetMyVideoFeedAsync(int userId, PublicationType publicationType, DateFilterType? dateFilterType = null);

        #endregion Publications

        #region Users

        Task GetCurrentUserAsync();

        Task UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        Task<UserDataModel> SendAvatarAsync(string path);

        #endregion Users

        #region Video

        Task<VideoMetadataDataModel> SendVideoAsync(int orderId, string path, string title, string description);

        #endregion Video
    }
}
