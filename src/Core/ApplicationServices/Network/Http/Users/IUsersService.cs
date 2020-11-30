using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users
{
    public interface IUsersService
    {
        Task VerifyEmailAsync();

        Task GetCurrentUserAsync();

        Task<BaseBundleApiModel<UserApiModel>> GetSubscriptionsAsync(int userId, int page, int pageSize);

        Task<BaseBundleApiModel<UserApiModel>> GetSubscribersAsync(int userId, int page, int pageSize);

        Task<UserApiModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        Task<UserApiModel> SendAvatarAsync(string path);

        Task ComplainUserAsync(int userId, string title, string description);

        Task<UserApiModel> GetUserAsync(int userId);

        Task<UserApiModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<UserApiModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<DocumentApiModel> SendVerifyDocumentAsync(string path);

        Task<CardApiModel> SaveCardAsync(string number, string userName);

        Task<CardApiModel> GetCardsAsync();

        Task DeleteCardAsync(int id);
    }
}