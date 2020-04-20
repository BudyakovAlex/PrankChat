using System.Threading.Tasks;
using Android.App;
using MvvmCross.Platforms.Android;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Droid.ApplicationServices.Callback;
using PrankChat.Mobile.Droid.ApplicationServices.Callbacks;
using VKontakte;
using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Login;

namespace PrankChat.Mobile.Droid.ApplicationServices
{
    public class ExternalAuthService : IExternalAuthService
    {
        //TODO: move to config
        private const string FacebookAppId = "621471715102522";

        private readonly IMvxAndroidCurrentTopActivity _mvxAndroidCurrentTopActivity;

        //TODO: move to config
        private readonly string[] _facebookPermissions = { @"public_profile", @"email" };
        private readonly string[] _vkontaktePermissions = { VKScope.Email, VKScope.Offline };

        public ExternalAuthService(IMvxAndroidCurrentTopActivity mvxAndroidCurrentTopActivity)
        {
            _mvxAndroidCurrentTopActivity = mvxAndroidCurrentTopActivity;

            FacebookSdk.ApplicationId = FacebookAppId;
            FacebookSdk.SdkInitialize(Application.Context.ApplicationContext);
        }

        public async Task<string> LoginWithFacebookAsync()
        {
            var currentContext = _mvxAndroidCurrentTopActivity.Activity ?? Xamarin.Essentials.Platform.CurrentActivity;
            if (currentContext is null)
            {
                return null;
            }

            var taskCompletionSource = new TaskCompletionSource<string>();
            FacebookCallback.Instance.CompletionSource = taskCompletionSource;
            FacebookCallback.Instance.Register();

            LoginManager.Instance.LogInWithReadPermissions(currentContext, _facebookPermissions);

            var token = await taskCompletionSource.Task;
            FacebookCallback.Instance.Unregister();

            return token;
        }

        public async Task<string> LoginWithVkontakteAsync()
        {
            var currentContext = _mvxAndroidCurrentTopActivity.Activity ?? Xamarin.Essentials.Platform.CurrentActivity;
            if (currentContext is null)
            {
                return null;
            }

            var taskCompletionSource = new TaskCompletionSource<string>();
            VkontakteCallback.Instance.CompletionSource = taskCompletionSource;

            VKSdk.Login(currentContext, _vkontaktePermissions);
            var token = await taskCompletionSource.Task;
            return token;
        }

        public void LogoutFromVkontakte()
        {
            VKSdk.Logout();
        }

        public void LogoutFromFacebook()
        {
            LoginManager.Instance.LogOut();
        }
    }
}