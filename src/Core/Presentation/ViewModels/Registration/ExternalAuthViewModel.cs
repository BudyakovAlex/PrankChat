using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class ExternalAuthViewModel : BasePageViewModel
    {
        private readonly IExternalAuthService _externalAuthService;
        private readonly IPushNotificationProvider _pushNotificationService;

        public ExternalAuthViewModel(
            IAuthorizationManager authorizationManager,
            IVersionManager versionManager,
            IUsersManager usersManager,
            IExternalAuthService externalAuthService,
            IPushNotificationProvider pushNotificationService)
        {
            AuthorizationManager = authorizationManager;
            VersionManager = versionManager;
            UsersManager = usersManager;
            _externalAuthService = externalAuthService;
            _pushNotificationService = pushNotificationService;
        }

        protected IAuthorizationManager AuthorizationManager { get; }
        protected IVersionManager VersionManager { get; }
        protected IUsersManager UsersManager { get; }

        protected virtual async Task<bool> TryLoginWithExternalServicesAsync(LoginType loginType)
        {
            var newActualVersion = await VersionManager.CheckAppVersionAsync();
            if (!string.IsNullOrEmpty(newActualVersion?.Link))
            {
                await NavigationManager.NavigateAsync<MaintananceViewModel, string>(newActualVersion?.Link);
                return false;
            }

            var token = loginType switch
            {
                LoginType.Vk => await _externalAuthService.LoginWithVkontakteAsync(),
                LoginType.Facebook => await _externalAuthService.LoginWithFacebookAsync(),
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var result = await AuthorizationManager.AuthorizeExternalAsync(token, loginType);
            return result;
        }

        protected virtual async Task NavigateAfterLoginAsync()
        {
            // TODO: not wait
            await UsersManager.GetCurrentUserAsync();
            await _pushNotificationService.TryUpdateTokenAsync();
            await NavigationManager.NavigateAsync<MainViewModel>();
        }
    }
}