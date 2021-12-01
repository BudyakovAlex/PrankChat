using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Providers.Platform;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Droid.Services;
using PrankChat.Mobile.Droid.PlatformBusinessServices.FileSystem;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video;
using PrankChat.Mobile.Droid.Bindings;
using PrankChat.Mobile.Droid.Presenters;
using PrankChat.Mobile.Droid.Providers;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.Core.Services.FileSystem;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Droid.Plugins.UserInteraction;
using MvvmCross.IoC;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using PrankChat.Mobile.Core.Services.Network;
using PrankChat.Mobile.Droid.Plugins.HttpClient;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAndroidSetup<Core.App>
    {
        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeLastChance(iocProvider);

            CompositionRoot.Container.RegisterType<IVideoPlayer, VideoPlayer>();
            CompositionRoot.Container.RegisterSingleton<IPlatformHttpClient, PlatformHttpClient>();
            CompositionRoot.Container.RegisterSingleton<IUserInteraction, UserInteraction>();
            CompositionRoot.Container.RegisterSingleton<IExternalAuthService, ExternalAuthService>();
            CompositionRoot.Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
            CompositionRoot.Container.RegisterSingleton<IPlatformPathsProvider, PlatformPathsProvider>();
            CompositionRoot.Container.RegisterSingleton<IFileSystemService, FileSystemService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<AppCompatButton>(nameof(OrderButtonStyleBinding), button => new OrderButtonStyleBinding(button));
            registry.RegisterCustomBindingFactory<View>(nameof(BackgroundBinding), view => new BackgroundBinding(view));
            registry.RegisterCustomBindingFactory<VideoView>(nameof(VideoUrlTargetBinding), view => new VideoUrlTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(nameof(ViewTouchTargetBinding), view => new ViewTouchTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(nameof(BackgroundColorBinding), view => new BackgroundColorBinding(view));
            registry.RegisterCustomBindingFactory<View>(nameof(BackgroundResourceBinding), view => new BackgroundResourceBinding(view));
            registry.RegisterCustomBindingFactory<ImageView>(nameof(ImageViewTintColorTargetBinding), view => new ImageViewTintColorTargetBinding(view));
            registry.RegisterCustomBindingFactory<TextView>(nameof(TextColorTargetBinding), view => new TextColorTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.StartPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.StartPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.EndPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.EndPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.TopPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.TopPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.BottomPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.BottomPadding));
            registry.RegisterCustomBindingFactory<TabLayout.Tab>(nameof(TabLayoutTabTextBinding), view => new TabLayoutTabTextBinding(view));
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Debug()
                .CreateLogger();
            return new SerilogLoggerFactory();
        }
    }
}