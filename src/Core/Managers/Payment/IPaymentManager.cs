using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Payment
{
    public interface IPaymentManager
    {
        Task<Models.Data.Payment> RefillAsync(double coast);

        Task<Withdrawal> WithdrawalAsync(double coast, int cardId);

        Task<Withdrawal[]> GetWithdrawalsAsync();

        Task CancelWithdrawalAsync(int withdrawalId);
    }
}