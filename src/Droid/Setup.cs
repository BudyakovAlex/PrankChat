using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Presenters;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;
using PrankChat.Mobile.Droid.ApplicationServices;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Crashlytic;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presenters;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IVideoPlayerService, VideoPlayerService>();
            Mvx.IoCProvider.RegisterType<ICrashlyticsService, CrashlyticsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IExternalAuthService, ExternalAuthService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISettingsService, SettingsService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<Button>(OrderButtonStyleBinding.TargetBinding, button => new OrderButtonStyleBinding(button));
            registry.RegisterCustomBindingFactory<View>(BackgroundBinding.TargetBinding, view => new BackgroundBinding(view));
            registry.RegisterCustomBindingFactory<VideoView>(VideoUrlTargetBinding.TargetBinding, view => new VideoUrlTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(ViewTouchTargetBinding.TargetBinding, view => new ViewTouchTargetBinding(view));
        }
    }
}
