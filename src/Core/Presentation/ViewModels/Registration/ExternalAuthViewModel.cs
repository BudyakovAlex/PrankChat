using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class ExternalAuthViewModel : BasePageViewModel
    {
        private readonly IExternalAuthService _externalAuthService;
        private readonly IPushNotificationService _pushNotificationService;

        public ExternalAuthViewModel(IExternalAuthService externalAuthService, IPushNotificationService pushNotificationService)
        {
            _externalAuthService = externalAuthService;
            _pushNotificationService = pushNotificationService;
        }

        protected virtual async Task<bool> TryLoginWithExternalServicesAsync(LoginType loginType)
        {
            var newActualVersion = await ApiService.CheckAppVersionAsync();
            if (!string.IsNullOrEmpty(newActualVersion?.Link))
            {
                await NavigationService.ShowMaintananceView(newActualVersion.Link);
                return false;
            }

            string token;
            switch (loginType)
            {
                case LoginType.Vk:
                    token = await _externalAuthService.LoginWithVkontakteAsync();
                    break;
                case LoginType.Facebook:
                    token = await _externalAuthService.LoginWithFacebookAsync();
                    break;
                default:
                    return false;
            }

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var result = await ApiService.AuthorizeExternalAsync(token, loginType);
            return result;
        }

        protected virtual async Task NavigateAfterLoginAsync()
        {
            // todo: not wait
            await ApiService.GetCurrentUserAsync();

            await _pushNotificationService.TryUpdateTokenAsync();

            await NavigationService.ShowMainView();
        }
    }
}