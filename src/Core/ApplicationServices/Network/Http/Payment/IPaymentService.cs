using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment
{
    public interface IPaymentService
    {
        Task<PaymentApiModel> RefillAsync(double coast);

        Task<WithdrawalApiModel> WithdrawalAsync(double coast, int cardId);

        Task<List<WithdrawalApiModel>> GetWithdrawalsAsync();

        Task CancelWithdrawalAsync(int withdrawalId);
    }
}