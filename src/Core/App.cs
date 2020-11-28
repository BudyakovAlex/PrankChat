using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Competitions;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Payment;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Publications;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Search;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Video;
using PrankChat.Mobile.Core.ApplicationServices.Notifications;
using PrankChat.Mobile.Core.ApplicationServices.Permissions;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.BusinessServices.Sentry;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers.BackgroundTasks.SendLogs;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Payment;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Search;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Managers.Common;
using System.Linq;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        private const string MappingProfileSuffix = "MappingProfile";

        public override void Initialize()
        {           
            InitializeMappings();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IWalkthroughsProvider, WalkthroughsProvider>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISendLogsBackgroundTask, SendLogsBackgroundTask>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IBackgroundTaskScheduler, BackgroundTaskScheduler>();

            RegisterServices();
            RegisterManagers();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPushNotificationProvider, PushNotificationProvider>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<INotificationBageViewModel, NotificationBageViewModel>();

            RegisterCustomAppStart<CustomAppStart>();
        }

        private void RegisterServices()
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ITimerService, TimerService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INavigationService, NavigationService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ILogger, Logger>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISentryService, SentryService>();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAuthorizationService, AuthorizationService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILogsService, LogsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IVersionService, VersionService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ICompetitionsService, CompetitionsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INotificationsService, NotificationsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IOrdersService, OrdersService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPaymentService, PaymentService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPublicationsService, PublicationsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISearchService, SearchService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IUsersService, UsersService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IVideoService, VideoService>();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<IErrorHandleService, ErrorHandleService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformService, PlatformService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPermissionService, PermissionService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMediaService, MediaService>();
        }

        private void RegisterManagers()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAuthorizationManager, AuthorizationManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILogsManager, LogsManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IVersionManager, VersionManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ICompetitionsManager, CompetitionsManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INotificationsManager, NotificationsManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IOrdersManager, OrdersManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPaymentManager, PaymentManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPublicationsManager, PublicationsManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISearchManager, SearchManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IUsersManager, UsersManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IVideoManager, VideoManager>();
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