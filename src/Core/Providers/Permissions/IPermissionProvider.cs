using System;
using System.Threading.Tasks;
using static Xamarin.Essentials.Permissions;

namespace PrankChat.Mobile.Core.Providers.Permissions
{
    public interface IPermissionProvider
    {
        Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new();
    }
}
