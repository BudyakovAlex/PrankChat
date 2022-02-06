using PrankChat.Mobile.Core.Services.Network.Http.Payment;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using PrankChat.Mobile.Core.Services.Analytics;
using PrankChat.Mobile.Core.Data.Enums;

namespace PrankChat.Mobile.Core.Managers.Payment
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentService _paymentService;
        private readonly IAnalyticsService _analyticsService;

        public PaymentManager(IPaymentService paymentService, IAnalyticsService analyticsService)
        {
            _paymentService = paymentService;
            _analyticsService = analyticsService;
        }

        public async Task<Models.Data.Payment> RefillAsync(double coast)
        {
            var response = await _paymentService.RefillAsync(coast);

            var payment = response.Map();
            if (payment != null)
            {
                _analyticsService.Track(AnalyticsEvent.Refill);
            }

            return payment;
        }

        public async Task<Withdrawal> WithdrawalAsync(double coast, int cardId)
        {
            var response = await _paymentService.WithdrawalAsync(coast, cardId);
            return response.Map();
        }

        public async Task<Withdrawal[]> GetWithdrawalsAsync()
        {
            var response = await _paymentService.GetWithdrawalsAsync();
            return response?.Select(withdrawals => withdrawals.Map()).ToArray() ?? Array.Empty<Withdrawal>();
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _paymentService.CancelWithdrawalAsync(withdrawalId);
        }
    }
}