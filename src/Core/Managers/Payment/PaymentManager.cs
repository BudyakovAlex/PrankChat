using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Managers.Payment
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentService _paymentService;

        public PaymentManager(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public Task<PaymentDataModel> RefillAsync(double coast)
        {
            return _paymentService.RefillAsync(coast);
        }

        public Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId)
        {
            return _paymentService.WithdrawalAsync(coast, cardId);
        }

        public Task<List<WithdrawalDataModel>> GetWithdrawalsAsync()
        {
            return _paymentService.GetWithdrawalsAsync();
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _paymentService.CancelWithdrawalAsync(withdrawalId);
        }
    }
}