using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment
{
    public interface IPaymentService
    {
        Task<PaymentDataModel> RefillAsync(double coast);

        Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId);

        Task<List<WithdrawalDataModel>> GetWithdrawalsAsync();

        Task CancelWithdrawalAsync(int withdrawalId);
    }
}