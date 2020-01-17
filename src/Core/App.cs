using System.Linq;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Storages;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            InitializeMappings();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INavigationService, NavigationService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IStorageService, StorageService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISettingsService, SettingsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IApiService, ApiService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IErrorHandleService, ErrorHandleService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();

            RegisterCustomAppStart<CustomAppStart>();
        }

        private void InitializeMappings()
        {
            var mappingTypes = CreatableTypes().EndingWith("MappingProfile").AsTypes().Select(c => c.ImplementationType);
            MappingConfig.Configure(mappingTypes);
        }
    }
}
