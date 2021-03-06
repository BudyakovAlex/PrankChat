using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Managers.Media;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Managers.Notifications;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.Managers.Payment;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Search;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Plugins.Timer;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.Permissions;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.Analytics;
using PrankChat.Mobile.Core.Services.ErrorHandling;
using PrankChat.Mobile.Core.Services.Network.Http.Authorization;
using PrankChat.Mobile.Core.Services.Network.Http.Common;
using PrankChat.Mobile.Core.Services.Network.Http.Competitions;
using PrankChat.Mobile.Core.Services.Network.Http.Notifications;
using PrankChat.Mobile.Core.Services.Network.Http.Payment;
using PrankChat.Mobile.Core.Services.Network.Http.Publications;
using PrankChat.Mobile.Core.Services.Network.Http.Search;
using PrankChat.Mobile.Core.Services.Network.Http.Users;
using PrankChat.Mobile.Core.Services.Network.Http.Video;
using PrankChat.Mobile.Core.Services.Notifications;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Common.Items;
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
            //NOTE: do not remove, need to avoid issues with resolve messenger
            Container.RegisterSingleton<IMvxMessenger, MvxMessengerHub>();

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
            Container.RegisterSingleton<IAnalyticsService, AnalyticsService>();
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
            Container.RegisterSingleton<IMediaManager, MediaManager>();
        }

        private void RegisterDependencies()
        {
            Container.RegisterSingleton<ISystemTimer, SystemTimer>();
            Container.RegisterSingleton(Container.IocConstruct<NotificationBadgeViewModel>());
            Container.RegisterSingleton(Container.IocConstruct<InviteFriendItemViewModel>());
        }

        private void RegisterProviders()
        {
            Container.RegisterSingleton<IEnvironmentConfigurationProvider, EnvironmentConfigurationProvider>();
            Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
            Container.RegisterSingleton<IPushNotificationProvider, PushNotificationProvider>();
            Container.RegisterSingleton<IWalkthroughsProvider, WalkthroughsProvider>();
            Container.RegisterSingleton<IPermissionProvider, PermissionProvider>();
        }
    }
}
