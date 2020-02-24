using System.Threading.Tasks;
using Android.App;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using Xamarin.Facebook;

namespace PrankChat.Mobile.Droid.ApplicationServices
{
    public class ExternalAuthService : IExternalAuthService
    {
        public void InitializeFacebookAsync()
        {
            FacebookSdk.ApplicationId = "1052355475141532";
            FacebookSdk.SdkInitialize(Application.Context.ApplicationContext);
        }

        public Task<string> LoginWithFacebookAsync()
        {
            return AndroidFacebookService.Instance.Login();
        }

        public void InitializeVkontakteAsync()
        {
        }

        public Task<string> LoginWithVkontakteAsync()
        {
            return AndroidVkService.Instance.Login();
        }
    }
}
