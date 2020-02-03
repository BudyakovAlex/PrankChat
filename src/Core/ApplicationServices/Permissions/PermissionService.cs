using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace PrankChat.Mobile.Core.ApplicationServices.Permissions
{
    public class PermissionService : IPermissionService
    {
        public async Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<TPermission>();
            if (permissionStatus != PermissionStatus.Granted)
                permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<TPermission>();

            if (permissionStatus == PermissionStatus.Granted || permissionStatus == PermissionStatus.Unknown)
                return true;

            return false;
        }
    }
}
