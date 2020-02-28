using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Providers;

namespace PrankChat.Mobile.Droid.Presentation.Views.Video
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation)]
    [MvxActivityPresentation]
    public class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private ExtendedVideoView _videoView;
        private FrameLayout _rootView;
        private LinearLayout _topPanel;
        private CustomMediaControllerView _mediaController;
        private ImageView _backImageView;
        private TextView _titleTextView;
        private TextView _descriptionTextView;
        private View _profileView;
        private MvxCachedImageView _profileImageView;
        private View _likeView;
        private TextView _likeTextView;
        private ImageView _shareImageView;

        private int _currentPosition;

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            _currentPosition = _videoView.CurrentPosition;
            base.OnConfigurationChanged(newConfig);
        }

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
            _rootView = FindViewById<FrameLayout>(Resource.Id.root_view);
            _backImageView = FindViewById<ImageView>(Resource.Id.back_image_view);
            _titleTextView = FindViewById<TextView>(Resource.Id.video_title_text_view);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.video_description_text_view);
            _topPanel = FindViewById<LinearLayout>(Resource.Id.top_panel);
            _profileView = FindViewById<View>(Resource.Id.profile_view);
            _likeView = FindViewById<View>(Resource.Id.like_view);
            _likeTextView = FindViewById<TextView>(Resource.Id.like_text_view);
            _shareImageView = FindViewById<ImageView>(Resource.Id.share_image_view);

            _profileImageView = FindViewById<MvxCachedImageView>(Resource.Id.profile_image_view);
            _profileImageView.ClipToOutline = true;
            _profileImageView.OutlineProvider = new OutlineProvider((view, outline) =>
            {
                outline.SetRoundRect(0, 0, view.Width, view.Height, view.Width / 2);
            });

            _videoView = FindViewById<ExtendedVideoView>(Resource.Id.video_view);
            _videoView.SetOnPreparedListener(new MediaPlayerOnPreparedListener(OnMediaPlayerPrepared));

            _mediaController = new CustomMediaControllerView(this)
            {
                Visibility = ViewStates.Gone,
                VideoView = _videoView,
                ViewStateChanged = (viewState) => _topPanel.Visibility = viewState
            };

            _mediaController.SetAnchorView(_rootView);
            _videoView.RequestFocus();
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(_videoView)
                      .For(VideoUrlTargetBinding.TargetBinding)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(_mediaController)
                      .For(v => v.IsMuted)
                      .To(vm => vm.IsMuted);

            bindingSet.Bind(_backImageView)
                      .For(v => v.BindClick())
                      .To(vm => vm.GoBackCommand);

            bindingSet.Bind(_titleTextView)
                     .For(v => v.Text)
                     .To(vm => vm.VideoName);

            bindingSet.Bind(_descriptionTextView)
                     .For(v => v.Text)
                     .To(vm => vm.Description);

            // TODO: add show profile click binding
            //bindingSet.Bind(_profileView)
            //          .For(v => v.BindClick())
            //          .To(vm => vm.)


            // TODO: add profile image binding
            //bindingSet.Bind(_profileImageView)
            //          .For(v => v.ImagePath)
            //          .To(vm => vm.);

            bindingSet.Bind(_likeView)
                      .For(v => v.BindClick())
                      .To(vm => vm.LikeCommand);

            bindingSet.Bind(_likeView)
                      .For(v => v.Activated)
                      .To(vm => vm.IsLiked);

            bindingSet.Bind(_likeTextView)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfLikesPresentation);

            bindingSet.Bind(_shareImageView)
                      .For(v => v.BindClick())
                      .To(vm => vm.ShareCommand);

            bindingSet.Apply();
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

        private void OnMediaPlayerPrepared(MediaPlayer mediaPlayer)
        {
            _mediaController.MediaPlayer = mediaPlayer;
            _mediaController.MediaPlayer.Looping = true;
        }
    }
}