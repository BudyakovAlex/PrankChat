using Android.Views;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Droid.Presentation.Bindings;

namespace PrankChat.Mobile.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.IoCProvider.RegisterType<IVideoPlayerService, Lalka>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<View>(BackgroundBinding.PropertyName, view => new BackgroundBinding(view));
        }
    }

    public class Lalka : IVideoPlayerService
    {
        public bool Muted { get => Player.Muted; set => Player.Muted = value; }

        public IVideoPlayer Player => new Ololo();

        public void Pause()
        {
        }

        public void Play(string uri)
        {
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }

    public class Ololo : IVideoPlayer
    {
        public bool IsPlaying => false;

        public bool Muted { get => true; set { } }

        public void Dispose()
        {
        }

        public void EnableRepeat(int repeatDelayInSeconds)
        {
        }

        public void Pause()
        {
         
        }

        public void Play()
        {
         
        }

        public void SetPlatformVideoPlayerContainer(object container)
        {
         
        }

        public void SetSourceUri(string uri)
        {
         
        }

        public void Stop()
        {
         
        }
    }
}
