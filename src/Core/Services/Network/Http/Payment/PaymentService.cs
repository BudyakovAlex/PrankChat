﻿using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Services.Network.Http.Payment;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.Network.Http.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public PaymentService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            _log = logProvider.GetLogFor<PaymentService>();

            var environment = environmentConfigurationProvider.Environment;

            _client = new HttpClient(environment.ApiUrl,
                                     environment.ApiVersion,
                                     userSessionProvider,
                                     _log,
                                     messenger);
        }

        public async Task<PaymentDto> RefillAsync(double coast)
        {
            var refillApiData = new RefillDto
            {
                Amount = coast
            };

            var data = await _client.PostAsync<RefillDto, ResponseDto<PaymentDto>>("payment", refillApiData);
            return data?.Data;
        }

        public async Task<WithdrawalDto> WithdrawalAsync(double coast, int cardId)
        {
            var createWithdrawalApiModel = new CreateWithdrawalDto()
            {
                Amount = coast,
                CreditCardId = cardId,
            };

            var dataApiModel = await _client.PostAsync<CreateWithdrawalDto, ResponseDto<WithdrawalDto>>("withdrawal", createWithdrawalApiModel);
            return dataApiModel?.Data;
        }

        public async Task<List<WithdrawalDto>> GetWithdrawalsAsync()
        {
            var dataApiModel = await _client.GetAsync<ResponseDto<List<WithdrawalDto>>>("withdrawal");
            return dataApiModel?.Data;
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _client.DeleteAsync($"withdrawal/{withdrawalId}", true);
        }
    }
}