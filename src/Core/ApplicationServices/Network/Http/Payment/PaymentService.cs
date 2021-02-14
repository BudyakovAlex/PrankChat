using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment
{
    public class PaymentService : BaseRestService, IPaymentService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public PaymentService(ISettingsService settingsService,
                              IAuthorizationService authorizeService,
                              IMvxLogProvider logProvider,
                              IMvxMessenger messenger,
                              ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _messenger = messenger;
            _log = logProvider.GetLogFor<PaymentService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
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