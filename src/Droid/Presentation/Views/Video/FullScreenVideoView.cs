using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Video
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation)]
    [MvxActivityPresentation]
    public class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private ExtendedVideoView videoView;
        private FrameLayout rootView;
        private CustomMediaControllerView mediaController;

        private int currentPosition;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            var decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen
                | SystemUiFlags.HideNavigation
                | SystemUiFlags.Immersive
                | SystemUiFlags.ImmersiveSticky);

            base.OnCreate(bundle, Resource.Layout.activity_full_screen_video);
            RequestedOrientation = ScreenOrientation.FullSensor;
        }

        protected override void SetViewProperties()
        {
            videoView = FindViewById<ExtendedVideoView>(Resource.Id.video_view);
            rootView = FindViewById<FrameLayout>(Resource.Id.root_view);

            mediaController = new CustomMediaControllerView(this) { VideoView = videoView };
            videoView.SetOnPreparedListener(new MediaPlayerOnPreparedListener((mp) => mediaController.MediaPlayer = mp));

            mediaController.SetAnchorView(rootView);
            videoView.RequestFocus();
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(videoView).For(VideoUrlTargetBinding.PropertyName).To(vm => vm.VideoUrl);
            bindingSet.Bind(mediaController).For(v => v.IsMuted).To(vm => vm.IsMuted);

            bindingSet.Apply();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            currentPosition = videoView.CurrentPosition;
            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(nameof(videoView.CurrentPosition), currentPosition);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            var position = savedInstanceState.GetInt(nameof(videoView.CurrentPosition), 0);
            videoView.SeekTo(position);
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}