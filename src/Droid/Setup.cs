using Android.Views;
using Android.Widget;
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
            base.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<View>(BackgroundBinding.PropertyName, view => new BackgroundBinding(view));
            registry.RegisterCustomBindingFactory<VideoView>(VideoUrlTargetBinding.PropertyName, view => new VideoUrlTargetBinding(view));
            registry.RegisterCustomBindingFactory<View>(ViewTouchTargetBinding.PropertyName, view => new ViewTouchTargetBinding(view));
        }
    }
}
