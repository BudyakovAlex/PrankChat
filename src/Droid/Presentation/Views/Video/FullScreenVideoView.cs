using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
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
        private ExtendedVideoView _videoView;
        private FrameLayout _rootView;
        private FrameLayout _topPanel;
        private CustomMediaControllerView _mediaController;
        private ImageView _backImageView;

        private int _currentPosition;

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
            _topPanel = FindViewById<FrameLayout>(Resource.Id.top_panel);
            _videoView = FindViewById<ExtendedVideoView>(Resource.Id.video_view);
            _rootView = FindViewById<FrameLayout>(Resource.Id.root_view);
            _backImageView = FindViewById<ImageView>(Resource.Id.back_image_view);
            _topPanel.Visibility = ViewStates.Gone;

            _mediaController = new CustomMediaControllerView(this)
            {
                VideoView = _videoView,
                ViewStateChanged = (viewState) => _topPanel.Visibility = viewState
            };

            _videoView.SetOnPreparedListener(new MediaPlayerOnPreparedListener((mp) => _mediaController.MediaPlayer = mp));

            _mediaController.SetAnchorView(_rootView);
            _videoView.RequestFocus();
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(_videoView)
                      .For(VideoUrlTargetBinding.PropertyName)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(_mediaController)
                      .For(v => v.IsMuted)
                      .To(vm => vm.IsMuted);
            bindingSet.Bind(_backImageView)
                      .For(v => v.BindClick())
                      .To(vm => vm.GoBackCommand);

            bindingSet.Apply();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            _currentPosition = _videoView.CurrentPosition;
            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(nameof(_videoView.CurrentPosition), _currentPosition);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            var position = savedInstanceState.GetInt(nameof(_videoView.CurrentPosition), 0);
            _videoView.SeekTo(position);
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}