using Android.Views;
using Android.Webkit;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IVideoPlayerService, VideoPlayerService>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(typeof(BackgroundBinding), typeof(View), BackgroundBinding.TargetBinding);
            base.FillTargetFactories(registry);
        }
    }
}
