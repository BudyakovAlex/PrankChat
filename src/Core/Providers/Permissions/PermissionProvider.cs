using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Providers.Permissions
{
    public class PermissionProvider : IPermissionProvider
    {
        public async Task<bool> RequestPermissionAsync<TPermission>() where TPermission : Xamarin.Essentials.Permissions.BasePermission, new()
        {
            var permissionStatus = await Xamarin.Essentials.Permissions.CheckStatusAsync<TPermission>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Xamarin.Essentials.Permissions.RequestAsync<TPermission>();
            }

            if (permissionStatus == PermissionStatus.Granted ||
                permissionStatus == PermissionStatus.Unknown)
            {
                return true;
            }

            return false;
        }
    }
}
