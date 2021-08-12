using System;
using System.Threading.Tasks;
using static Xamarin.Essentials.Permissions;

namespace PrankChat.Mobile.Core.Services.Permissions
{
    public interface IPermissionService
    {
        Task<bool> RequestPermissionAsync<TPermission>() where TPermission : BasePermission, new();
    }
}
