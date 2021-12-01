using Javax.Net.Ssl;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient.HostnameVerfier
{
    public class AllowAllSSLHostnamVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }
    }
}
