using System.Threading.Tasks;
using Android.Content;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace PrankChat.Mobile.Droid.Services.Callbacks
{
    public class FacebookCallback : Java.Lang.Object, GraphRequest.IGraphJSONObjectCallback, GraphRequest.ICallback, IFacebookCallback
    {
        private ICallbackManager _callbackManager;

        public void Register()
        {
            _callbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(_callbackManager, this);
        }

        public void Unregister()
        {
            if (_callbackManager is null)
            {
                return;
            }

            LoginManager.Instance.UnregisterCallback(_callbackManager);
            _callbackManager = null;
        }

        public static FacebookCallback Instance { get; } = new FacebookCallback();

        public TaskCompletionSource<string> CompletionSource { get; set; }

        public bool OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            try
            {
                return _callbackManager?.OnActivityResult(requestCode, resultCode, data) ?? false;
            }
            catch
            {
                return false;
            }
        }

        public void OnCompleted(JSONObject data, GraphResponse response)
        {
            OnCompleted(response);
        }

        public void OnCompleted(GraphResponse response)
        {
            if (response?.JSONObject == null)
            {
                CompletionSource?.TrySetResult(null);
                return;
            }

            CompletionSource?.TrySetResult(AccessToken.CurrentAccessToken.Token);
        }

        public void OnCancel()
        {
            CompletionSource?.TrySetResult(null);
        }

        public void OnError(FacebookException exception)
        {
            CompletionSource?.TrySetResult(null);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            if (result is LoginResult loginResult)
            {
                CompletionSource?.TrySetResult(loginResult.AccessToken.Token);
                return;
            }

            CompletionSource?.TrySetResult(null);
        }
    }
}
