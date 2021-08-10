using PrankChat.Mobile.Core.Services.Network.Http.Payment;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Payment
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentService _paymentService;

        public PaymentManager(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Models.Data.Payment> RefillAsync(double coast)
        {
            var response = await _paymentService.RefillAsync(coast);
            return response.Map();
        }

        public async Task<Withdrawal> WithdrawalAsync(double coast, int cardId)
        {
            var response = await _paymentService.WithdrawalAsync(coast, cardId);
            return response.Map();
        }

        public async Task<List<Withdrawal>> GetWithdrawalsAsync()
        {
            var response = await _paymentService.GetWithdrawalsAsync();
            return response.Select(withdrawals => withdrawals.Map()).ToList();
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _paymentService.CancelWithdrawalAsync(withdrawalId);
        }
    }
}