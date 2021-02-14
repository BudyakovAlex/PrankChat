using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Providers.UserSession;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract
{
    public class BaseRestService
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;
        private readonly IAuthorizationService _authorizeService;

        private readonly HttpClient _client;

        public BaseRestService(IUserSessionProvider userSessionProvider,
                               IAuthorizationService authorizeService,
                               IMvxLogProvider logProvider,
                               IMvxMessenger messenger,
                               ILogger logger)
        {
            _userSessionProvider = userSessionProvider;
            _messenger = messenger;
            _log = logProvider.GetLogFor<BaseRestService>();
            _authorizeService = authorizeService;

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     userSessionProvider,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_userSessionProvider.User == null)
            {
                return;
            }

            _authorizeService.RefreshTokenAsync().FireAndForget();
        }
    }
}