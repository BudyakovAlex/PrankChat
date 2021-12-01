using Java.Net;
using Java.Security.Cert;
using Javax.Net.Ssl;

namespace PrankChat.Mobile.Droid.Plugins.HttpClient.TrustManagers
{
    public class AllowAllSSLX509ExtendedTrustManager : X509ExtendedTrustManager
    {
        public override void CheckClientTrusted(X509Certificate[] chain, string authType, Socket socket)
        {
        }

        public override void CheckClientTrusted(X509Certificate[] chain, string authType, SSLEngine engine)
        {
        }

        public override void CheckClientTrusted(X509Certificate[] chain, string authType)
        {
        }

        public override void CheckServerTrusted(X509Certificate[] chain, string authType, Socket socket)
        {
        }

        public override void CheckServerTrusted(X509Certificate[] chain, string authType, SSLEngine engine)
        {
        }

        public override void CheckServerTrusted(X509Certificate[] chain, string authType)
        {
        }

        public override X509Certificate[] GetAcceptedIssuers()
        {
            return new X509Certificate[0];
        }
    }
}
