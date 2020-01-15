using System.Collections.Generic;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public interface IApiService
    {
        Task AuthorizeAsync(string email, string password);

        Task RegisterAsync(UserRegistrationDataModel userInfo);

        Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task<List<OrderDataModel>> GetOrdersAsync();
        
        Task<VideoMetadataBundleDataModel> GetVideoFeedAsync();
    }
}
