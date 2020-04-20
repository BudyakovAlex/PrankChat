using System.Threading.Tasks;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class ExternalAuthViewModel : BaseViewModel
    {
        private readonly IExternalAuthService _externalAuthService;
        private readonly IPushNotificationService _pushNotificationService;

        public ExternalAuthViewModel(INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService,
                                     IExternalAuthService externalAuthService,
                                     IPushNotificationService pushNotificationService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _externalAuthService = externalAuthService;
            _pushNotificationService = pushNotificationService;
        }

        protected virtual async Task<bool> TryLoginWithExternalServicesAsync(LoginType loginType)
        {
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