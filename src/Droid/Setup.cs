using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.AppCenter;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Droid.ApplicationServices;
using PrankChat.Mobile.Droid.PlatformBusinessServices.AppCenter;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presenters;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAndroidSetup<Core.App>
    {
        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            CompositionRoot.Container.RegisterType<IVideoPlayerService, VideoPlayerService>();
            CompositionRoot.Container.RegisterSingleton<IAppCenterService, AppCenterService>();
            CompositionRoot.Container.RegisterSingleton<IDialogService, DialogService>();
            CompositionRoot.Container.RegisterSingleton<IExternalAuthService, ExternalAuthService>();
            CompositionRoot.Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<AppCompatButton>(OrderButtonStyleBinding.TargetBinding, button => new OrderButtonStyleBinding(button));
            registry.RegisterCustomBindingFactory<View>(BackgroundBinding.TargetBinding, view => new BackgroundBinding(view));
            registry.RegisterCustomBindingFactory<VideoView>(VideoUrlTargetBinding.TargetBinding, view => new VideoUrlTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(ViewTouchTargetBinding.TargetBinding, view => new ViewTouchTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(BackgroundColorBinding.TargetBinding, view => new BackgroundColorBinding(view));
            registry.RegisterCustomBindingFactory<View>(BackgroundResourceBinding.TargetBinding, view => new BackgroundResourceBinding(view));
            registry.RegisterCustomBindingFactory<ImageView>(ImageViewTintColorTargetBinding.TargetBinding, view => new ImageViewTintColorTargetBinding(view));
            registry.RegisterCustomBindingFactory<TextView>(TextColorTargetBinding.TargetBinding, view => new TextColorTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.StartPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.StartPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.EndPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.EndPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.TopPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.TopPadding));
            registry.RegisterCustomBindingFactory<View>(PaddingTargetBinding.BottomPadding, view => new PaddingTargetBinding(view, PaddingTargetBinding.BottomPadding));
            registry.RegisterCustomBindingFactory<TabLayout.Tab>(TabLayoutTabTextBinding.TargetBinding, view => new TabLayoutTabTextBinding(view));
        }
    }
}
