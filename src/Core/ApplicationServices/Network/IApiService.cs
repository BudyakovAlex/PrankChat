using System.Collections.Generic;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        #region Authorize 

        Task AuthorizeAsync(string email, string password);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        #endregion 

        #region Orders

        Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task<List<OrderDataModel>> GetOrdersAsync();

        Task<OrderDataModel> GetOrderDetailsAsync(int orderId);

        Task<OrderDataModel> TakeOrderAsync(int orderId, int executorId);

        Task CancelOrderAsync(int orderId);

        #endregion

        #region Publications

        Task<VideoMetadataBundleDataModel> GetVideoFeedAsync();

        #endregion

        #region Users

        Task GetCurrentUser();

        #endregion
    }
}
