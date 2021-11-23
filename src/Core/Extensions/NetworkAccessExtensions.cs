using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class NetworkAccessExtensions
    {
        public static bool HasConnection(this NetworkAccess networkAccess)
        {
            return networkAccess != NetworkAccess.None && networkAccess != NetworkAccess.ConstrainedInternet;
        }
    }
}