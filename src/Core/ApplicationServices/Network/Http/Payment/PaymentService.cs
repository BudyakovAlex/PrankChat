using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment
{
    public class PaymentService : BaseRestService, IPaymentService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public PaymentService(
            ISettingsService settingsService,
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

        public async Task<PaymentDataModel> RefillAsync(double coast)
        {
            var refillApiData = new RefillApiData
            {
                Amount = coast
            };

            var data = await _client.PostAsync<RefillApiData, DataApiModel<PaymentApiModel>>("payment", refillApiData);
            return MappingConfig.Mapper.Map<PaymentDataModel>(data?.Data);
        }

        public async Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId)
        {
            var createWithdrawalApiModel = new CreateWithdrawalApiModel()
            {
                Amount = coast,
                CreditCardId = cardId,
            };

            var dataApiModel = await _client.PostAsync<CreateWithdrawalApiModel, DataApiModel<WithdrawalApiModel>>("withdrawal", createWithdrawalApiModel);
            var data = MappingConfig.Mapper.Map<WithdrawalDataModel>(dataApiModel?.Data);
            return data;
        }

        public async Task<List<WithdrawalDataModel>> GetWithdrawalsAsync()
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<List<WithdrawalApiModel>>>("withdrawal");
            var data = MappingConfig.Mapper.Map<List<WithdrawalDataModel>>(dataApiModel?.Data);
            return data;
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _client.DeleteAsync($"withdrawal/{withdrawalId}", true);
        }
    }
}