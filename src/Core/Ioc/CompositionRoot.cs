using MvvmCross;
using MvvmCross.IoC;
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
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Payment;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Search;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Managers.Common;

namespace PrankChat.Mobile.Core.Ioc
{
    public class CompositionRoot
    {
        public CompositionRoot(bool shouldCreateNewContainer)
        {
            Container = ProduceProvider(shouldCreateNewContainer);
        }

        public static IIoCProvider Container { get; private set; }

        public void Initialize()
        {
            RegisterServices();
            RegisterManagers();
            RegisterProviders();
            RegisterDependencies();
        }

        private IIoCProvider ProduceProvider(bool shouldCreateNewContainer)
        {
            var mvxProvider = shouldCreateNewContainer
                   ? MvxIoCProvider.Initialize()
                   : Mvx.IoCProvider;

            return new IoCProvider(mvxProvider);
        }

        private void RegisterServices()
        {
            Container.RegisterSingleton<ITimerService, TimerService>();

            Container.RegisterSingleton<IAuthorizationService, AuthorizationService>();
            Container.RegisterSingleton<IVersionService, VersionService>();
            Container.RegisterSingleton<ICompetitionsService, CompetitionsService>();
            Container.RegisterSingleton<INotificationsService, NotificationsService>();
            Container.RegisterSingleton<IOrdersService, OrdersService>();
            Container.RegisterSingleton<IPaymentService, PaymentService>();
            Container.RegisterSingleton<IPublicationsService, PublicationsService>();
            Container.RegisterSingleton<ISearchService, SearchService>();
            Container.RegisterSingleton<IUsersService, UsersService>();
            Container.RegisterSingleton<IVideoService, VideoService>();

            Container.RegisterSingleton<IErrorHandleService, ErrorHandleService>();
            Container.RegisterSingleton<IPlatformService, PlatformService>();
            Container.RegisterSingleton<IPermissionService, PermissionService>();
            Container.RegisterSingleton<IMediaService, MediaService>();
        }

        private void RegisterManagers()
        {
            Container.RegisterSingleton<IUsersManager, UsersManager>();
            Container.RegisterSingleton<INavigationManager, NavigationManager>();
            Container.RegisterSingleton<IAuthorizationManager, AuthorizationManager>();
            Container.RegisterSingleton<IVersionManager, VersionManager>();
            Container.RegisterSingleton<ICompetitionsManager, CompetitionsManager>();
            Container.RegisterSingleton<INotificationsManager, NotificationsManager>();
            Container.RegisterSingleton<IOrdersManager, OrdersManager>();
            Container.RegisterSingleton<IPaymentManager, PaymentManager>();
            Container.RegisterSingleton<IPublicationsManager, PublicationsManager>();
            Container.RegisterSingleton<ISearchManager, SearchManager>();
            Container.RegisterSingleton<IVideoManager, VideoManager>();
        }

        private void RegisterDependencies()
        {
            Container.RegisterSingleton<INotificationBageViewModel, NotificationBageViewModel>();
        }

        private void RegisterProviders()
        {
            Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
            Container.RegisterSingleton<IPushNotificationProvider, PushNotificationProvider>();
            Container.RegisterSingleton<IWalkthroughsProvider, WalkthroughsProvider>();
        }
    }
}
