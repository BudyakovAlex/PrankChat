using System.Linq;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Permissions;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.BusinessServices.Sentry;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.SendLogs;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        private const string MappingProfileSuffix = "MappingProfile";

        public override void Initialize()
        {

           
            InitializeMappings();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<ITimerService, TimerService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INavigationService, NavigationService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IWalkthroughsProvider, WalkthroughsProvider>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ILogger, Logger>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISentryService, SentryService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISendLogsBackgroundTask, SendLogsBackgroundTask>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IBackgroundTaskScheduler, BackgroundTaskScheduler>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IApiService, ApiService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IErrorHandleService, ErrorHandleService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPermissionService, PermissionService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMediaService, MediaService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPushNotificationService, PushNotificationService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<INotificationBageViewModel, NotificationBageViewModel>();

            RegisterCustomAppStart<CustomAppStart>();


        }

        private void InitializeMappings()
        {
            var mappingTypes = CreatableTypes().EndingWith(MappingProfileSuffix)
                                               .AsTypes()
                                               .Select(serviceType => serviceType.ImplementationType);
            MappingConfig.Configure(mappingTypes);
        }
    }
}
