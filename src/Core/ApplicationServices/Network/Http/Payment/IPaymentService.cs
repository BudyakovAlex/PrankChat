using PrankChat.Mobile.Core.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment
{
    public interface IPaymentService
    {
        Task<PaymentDto> RefillAsync(double coast);

        Task<WithdrawalDto> WithdrawalAsync(double coast, int cardId);

        Task<List<WithdrawalDto>> GetWithdrawalsAsync();

        Task CancelWithdrawalAsync(int withdrawalId);
    }
}