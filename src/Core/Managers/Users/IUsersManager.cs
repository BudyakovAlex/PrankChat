using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Users
{
    public interface IUsersManager
    {
        Task VerifyEmailAsync();

        Task GetCurrentUserAsync();

        Task<PaginationModel<UserDataModel>> GetSubscriptionsAsync(int userId, int page, int pageSize);

        Task<PaginationModel<UserDataModel>> GetSubscribersAsync(int userId, int page, int pageSize);

        Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo);

        Task<UserDataModel> SendAvatarAsync(string path);

        Task ComplainUserAsync(int userId, string title, string description);

        Task<UserDataModel> GetUserAsync(int userId);

        Task<UserDataModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<UserDataModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<DocumentDataModel> SendVerifyDocumentAsync(string path);

        Task<CardDataModel> SaveCardAsync(string number, string userName);

        Task<CardDataModel> GetCardsAsync();

        Task DeleteCardAsync(int id);
    }
}