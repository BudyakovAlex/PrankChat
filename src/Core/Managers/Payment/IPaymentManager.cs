using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Payment
{
    public interface IPaymentManager
    {
        Task<PaymentDataModel> RefillAsync(double coast);

        Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId);

        Task<List<WithdrawalDataModel>> GetWithdrawalsAsync();

        Task CancelWithdrawalAsync(int withdrawalId);
    }
}