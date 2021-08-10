using System.Threading.Tasks;
using Facebook.LoginKit;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.iOS.ApplicationServices.Callbacks;
using PrankChat.Mobile.iOS.Utils.Helpers;
using VKontakte;

namespace PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly string[] _facebookPermissions = { @"public_profile", @"email" };

        private readonly string[] _permissions = { VKPermissions.Email, VKPermissions.Offline };

        private readonly LoginManager _facebookLoginManager;

        public ExternalAuthService()
        {
            _facebookLoginManager = new LoginManager();
            VKSdk.Initialize(AppDelegate.VkAppId);
        }

        public Task<string> LoginWithFacebookAsync()
        {
            _facebookLoginManager.LogOut();
            var completionSource = new TaskCompletionSource<string>();
            FacebookCallback.Instance.CompletionSource = completionSource;

            _facebookLoginManager.LogIn(_facebookPermissions, DeviceHelper.GetCurrentViewController(), FacebookCallback.Instance.LoginManagerLoginHandler);
            return completionSource.Task;
        }

        public async Task<string> LoginWithVkontakteAsync()
        {
            VKSdk.WakeUpSession(_permissions, (state, error) => {});
            VKSdk.Instance.RegisterDelegate(VkontakteDelegate.Instance);
            VKSdk.Instance.UiDelegate = VkontakteDelegate.Instance;

            var completionSource = new TaskCompletionSource<string>();
            VKSdk.Authorize(_permissions, VKAuthorizationOptions.DisableSafariController);
            VkontakteDelegate.Instance.CompletionSource = completionSource;

            var token = await completionSource.Task;
            VKSdk.Instance.UnregisterDelegate(VkontakteDelegate.Instance);
            VKSdk.Instance.UiDelegate = null;

            return token;
        }

        public void LogoutFromFacebook()
        {
            _facebookLoginManager.LogOut();
        }

        public void LogoutFromVkontakte()
        {
            VKSdk.ForceLogout();
        }

        public void Logout()
        {
            _facebookLoginManager.LogOut();
        }
    }
}