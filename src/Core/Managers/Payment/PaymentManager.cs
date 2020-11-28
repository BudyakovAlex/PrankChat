using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
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

        public async Task<PaymentDataModel> RefillAsync(double coast)
        {
            return await _paymentService.RefillAsync(coast);
        }

        public async Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId)
        {
            return await _paymentService.WithdrawalAsync(coast, cardId);
        }

        public async Task<List<WithdrawalDataModel>> GetWithdrawalsAsync()
        {
            return await _paymentService.GetWithdrawalsAsync();
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _paymentService.CancelWithdrawalAsync(withdrawalId);
        }
    }
}