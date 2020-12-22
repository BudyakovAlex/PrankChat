using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.Content.Resources;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Java.Lang;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using Android.Content.Res;

namespace PrankChat.Mobile.Droid.Presentation.Views.Video
{
    [Activity(ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Theme = "@style/Theme.PrankChat.Translucent")]
    [MvxActivityPresentation]
    public class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private const int AnimationDuration = 300;

        private ExtendedVideoView _videoView;
        private FrameLayout _rootView;
        private ConstraintLayout _topPanel;
        private CustomMediaControllerView _mediaController;
        private ImageView _backImageView;
        private TextView _titleTextView;
        private TextView _descriptionTextView;
        private View _profileView;
        private ImageView _subscriptionTagImageView;
        private CircleCachedImageView _profileImageView;
        private View _likeView;
        private ImageView _likesImageView;
        private TextView _likeTextView;
        private ImageView _shareImageView;
        private View _dislikeView;
        private ImageView _dislikesImageView;
        private TextView _dislikeTextView;
        private View _commentsView;
        private TextView _commentsTextView;

        private int _currentPosition;
        private float _lastY;
        private float _lastX;
        private float _firstX;
        private bool _isSwipeTriggered;
        private bool _isDisliked;
        private bool _isLiked;
        private bool _isSubscribed;

        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                if (_isLiked)
                {
                    _likesImageView.ImageTintList = ResourcesCompat.GetColorStateList(Resources, Resource.Color.accent, Theme);
                }
                else
                {
                    _likesImageView.ImageTintList = null;
                }
            }
        }

        public bool IsDisliked
        {
            get => _isDisliked;
            set
            {
                _isDisliked = value;
                if (_isDisliked)
                {
                    _dislikesImageView.ImageTintList = ResourcesCompat.GetColorStateList(Resources, Resource.Color.accent, Theme);
                }
                else
                {
                    _dislikesImageView.ImageTintList = null;
                }
            }
        }

        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                var id = _isSubscribed ? Resource.Drawable.ic_check_mark : Resource.Drawable.ic_plus;
                _subscriptionTagImageView.SetImageResource(id);
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            _currentPosition = _videoView.CurrentPosition;
            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            var decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility) (SystemUiFlags.Fullscreen
                | SystemUiFlags.HideNavigation
                | SystemUiFlags.Immersive
                | SystemUiFlags.ImmersiveSticky);

            base.OnCreate(bundle, Resource.Layout.activity_full_screen_video);
            RequestedOrientation = ScreenOrientation.FullSensor;
        }

        protected override void SetViewProperties()
        {
            _backImageView = FindViewById<ImageView>(Resource.Id.back_image_view);
            _titleTextView = FindViewById<TextView>(Resource.Id.video_title_text_view);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.video_description_text_view);
            _topPanel = FindViewById<ConstraintLayout>(Resource.Id.top_panel);
            _profileView = FindViewById<View>(Resource.Id.profile_view);
            _likeView = FindViewById<View>(Resource.Id.like_view);
            _likesImageView = FindViewById<ImageView>(Resource.Id.likes_image_view);
            _likeTextView = FindViewById<TextView>(Resource.Id.like_text_view);
            _shareImageView = FindViewById<ImageView>(Resource.Id.share_image_view);

            _dislikeView = FindViewById<View>(Resource.Id.dislike_view);
            _dislikesImageView = FindViewById<ImageView>(Resource.Id.dislikes_image_view);
            _dislikeTextView = FindViewById<TextView>(Resource.Id.dislike_text_view);

            _commentsView = FindViewById<View>(Resource.Id.comments_view);
            _commentsTextView = FindViewById<TextView>(Resource.Id.comments_text_view);

            _rootView = FindViewById<FrameLayout>(Resource.Id.root_view);
            _rootView.SetOnTouchListener(new ViewOnTouchListener(OnRootViewTouched));

            _subscriptionTagImageView = FindViewById<ImageView>(Resource.Id.subscription_tag_image_view);

            _profileImageView = FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);

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

            bindingSet.Bind(this).For(v => v.IsLiked).To(vm => vm.IsLiked);
            bindingSet.Bind(this).For(v => v.IsDisliked).To(vm => vm.IsDisliked);
            bindingSet.Bind(this).For(v => v.IsSubscribed).To(vm => vm.IsSubscribed);

            bindingSet.Bind(_videoView)
                      .For(VideoUrlTargetBinding.TargetBinding)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(_mediaController)
                      .For(v => v.IsMuted)
                      .To(vm => vm.IsMuted)
                      .TwoWay();

            bindingSet.Bind(_backImageView)
                      .For(v => v.BindClick())
                      .To(vm => vm.GoBackCommand);

            bindingSet.Bind(_titleTextView)
                      .For(v => v.Text)
                      .To(vm => vm.VideoName);

            bindingSet.Bind(_descriptionTextView)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(_profileView)
                      .For(v => v.BindClick())
                      .To(vm => vm.OpenUserProfileCommand);

            bindingSet.Bind(_profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl);

            bindingSet.Bind(_profileImageView)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortName);

            bindingSet.Bind(_likeView)
                      .For(v => v.BindClick())
                      .To(vm => vm.LikeCommand);

            bindingSet.Bind(_dislikeView)
                      .For(v => v.BindClick())
                      .To(vm => vm.DislikeCommand);

            bindingSet.Bind(_commentsView)
                      .For(v => v.BindClick())
                      .To(vm => vm.OpenCommentsCommand);

            bindingSet.Bind(_likeView)
                      .For(v => v.Clickable)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(_likeView)
                      .For(v => v.Enabled)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(_likeView)
                      .For(v => v.Activated)
                      .To(vm => vm.IsLiked);

            bindingSet.Bind(_likeTextView)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfLikesPresentation);

            bindingSet.Bind(_dislikeView)
                      .For(v => v.Clickable)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(_dislikeView)
                      .For(v => v.Enabled)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(_dislikeView)
                      .For(v => v.Activated)
                      .To(vm => vm.IsDisliked);

            bindingSet.Bind(_dislikeTextView)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfDislikesPresentation);

            bindingSet.Bind(_commentsTextView)
                     .For(v => v.Text)
                     .To(vm => vm.NumberOfCommentsPresentation);

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

        private void OnMediaPlayerPrepared(MediaPlayer mediaPlayer)
        {
            _mediaController.MediaPlayer = mediaPlayer;
            _mediaController.MediaPlayer.Looping = true;
        }

        private bool OnRootViewTouched(View view, MotionEvent motionEvent)
        {
            switch (motionEvent.Action)
            {
                case MotionEventActions.Down:
                    _lastY = motionEvent.RawY;
                    _firstX = motionEvent.RawX;
                    return true;

                case MotionEventActions.Move:
                    _isSwipeTriggered = true;
                    var y = view.TranslationY + (motionEvent.RawY - _lastY);
                    view.TranslationY = y > 0 ? y : 0;
                    _lastY = motionEvent.RawY;
                    _lastX = motionEvent.RawX;
                    return true;

                case MotionEventActions.Cancel:
                case MotionEventActions.Outside:
                case MotionEventActions.Up:
                    AnimateTranslation(view);
                    CanExecuteDirectedSwipes(view);
                    _rootView.PerformClick();
                    return true;

                default:
                    return false;
            }
        }

        private void AnimateTranslation(View view)
        {
            if (view.TranslationY < view.Height / 4)
            {
                view.Animate()
                    .TranslationY(0f)
                    .SetDuration(AnimationDuration)
                    .Start();
                return;
            }

            view.Animate()
                .TranslationY(view.Height)
                .SetDuration(AnimationDuration)
                .WithEndAction(new Runnable(() => ViewModel.GoBackCommand.ExecuteAsync()))
                .Start();
        }

        private void CanExecuteDirectedSwipes(View view)
        {
            if (!_isSwipeTriggered)
            {
                return;
            }

            if (_firstX < _lastX && _lastX - _firstX >= view.Width / 4)
            {
                ResetMediaPlayer();
                ViewModel.MovePreviousCommand.Execute();
                ResetSwipe();
                return;
            }

            if (_lastX < _firstX && _firstX - _lastX >= view.Width / 4)
            {
                ResetMediaPlayer();
                ViewModel.MoveNextCommand.Execute();
                ResetSwipe();
                return;
            }
        }

        private void ResetMediaPlayer()
        {
            if (_mediaController != null)
            {
                _mediaController.MediaPlayer = null;
            }
        }

        private void ResetSwipe()
        {
            _lastX = 0;
            _firstX = 0;
            _isSwipeTriggered = false;
        }
    }
}