using System.Linq;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Permissions;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            InitializeMappings();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<ITimerService, TimerService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INavigationService, NavigationService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISettingsService, SettingsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IApiService, ApiService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IErrorHandleService, ErrorHandleService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPermissionService, PermissionService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMediaService, MediaService>();

            RegisterCustomAppStart<CustomAppStart>();
        }

        private void InitializeMappings()
        {
            var mappingTypes = CreatableTypes().EndingWith("MappingProfile").AsTypes().Select(c => c.ImplementationType);
            MappingConfig.Configure(mappingTypes);
        }
    }
}
