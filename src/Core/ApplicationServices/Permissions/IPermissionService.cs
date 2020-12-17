using System;
using System.Threading.Tasks;
using static Xamarin.Essentials.Permissions;

namespace PrankChat.Mobile.Core.ApplicationServices.Permissions
{
    public interface IPermissionService
    {
        Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new();
    }
}
