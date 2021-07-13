using CoreGraphics;
using Foundation;
using LibVLCSharp.Platforms.iOS;
using LibVLCSharp.Shared;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.ViewModels;
using ObjCRuntime;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using System;
using System.Threading.Tasks;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Video
{
    [MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.FullScreen)]
    public partial class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private const int ThreeSecondsTicks = 30_000_000;
        private const double AnimationDuration = 0.3d;

        private bool _wasPlaying;
        private long _lastActionTicks;
        private nfloat _oldY;
        private nfloat _lastX;
        private nfloat _firstX;

        private bool _isSwipeTriggered;
        private VideoView _player;
        private UITapGestureRecognizer _tapGestureRecognizer;

        private IDisposable _onEndReachedSubscription;
        private IDisposable _onTimeChangedSubscription;
        private IDisposable _onBufferingSubscription;
        private IDisposable _onPausedSubscription;
        private IDisposable _onPlayingSubscription;
        private IDisposable _onStoppedSubscription;
        private IDisposable _onMutedChangedSubscription;

        public event EventHandler IsMutedChanged;

        private MvxInteraction _interaction;
        public MvxInteraction Interaction
        {
            get => _interaction;
            set
            {
                if (_interaction != null)
                {
                    _interaction.Requested -= OnInteractionRequested;
                }

                _interaction = value;

                if (_interaction != null)
                {
                    _interaction.Requested += OnInteractionRequested;
                }
            }
        }

        private IVideoPlayer _videoPlayer;
        public IVideoPlayer VideoPlayer
        {
            get => _videoPlayer;
            set
            {
                _videoPlayer = value;
                if (_videoPlayer is null)
                {
                    return;
                }

                Initialize();
            }
        }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                likeImageView.TintColor = _isLiked ? Theme.Color.Accent : Theme.Color.White;
            }
        }

        public bool _isDisliked;
        public bool IsDisliked
        {
            get => _isDisliked;
            set
            {
                _isDisliked = value;
                dislikeImageView.TintColor = _isDisliked ? Theme.Color.Accent : Theme.Color.White;
            }
        }

        private bool _isSubscribed;
        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;

                var imageName = _isSubscribed ? "ic_check_mark" : "ic_plus";
                subscriptionTagImageView.Image = UIImage.FromBundle(imageName);
            }
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_videoPlayer is null)
                {
                    return;
                }

                _videoPlayer.IsMuted = value;
            }
        }

        private bool IsPlayerInvalid =>
            _player?.MediaPlayer?.Media == null ||
            _player.MediaPlayer.State == VLCState.Error ||
            _player.MediaPlayer.Media?.Duration < 0;

        public override void ViewDidLoad()
        {
            IsRotateEnabled = true;

            base.ViewDidLoad();

            NSNotificationCenter.DefaultCenter.AddObserver(this, new Selector(nameof(WillResignActive)), UIApplication.WillResignActiveNotification, null);
            NSNotificationCenter.DefaultCenter.AddObserver(this, new Selector(nameof(DidBecomeActive)), UIApplication.DidBecomeActiveNotification, null);
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            coordinator.AnimateAlongsideTransition(context =>
            {
                PlayerBufferingChanged();
                UpdateTimePassedWidthConstraint();
            }, null);

            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        protected override void SetupControls()
        {
            likeImageView.Image = likeImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            dislikeImageView.Image = dislikeImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            overlayView.AddGestureRecognizer(new UITapGestureRecognizer(_ => overlayView.Hidden = true));
            playButton.AddGestureRecognizer(new UITapGestureRecognizer(PlayButtonTap));
            muteButton.AddGestureRecognizer(new UITapGestureRecognizer(MuteButtonTap));
            closeButton.AddGestureRecognizer(new UITapGestureRecognizer(Close));
            watchProgressControlContainer.AddGestureRecognizer(new UIPanGestureRecognizer(WatchProgressControlContainerPan));
            View.AddGestureRecognizer(new UIPanGestureRecognizer(ViewPan));
            _tapGestureRecognizer = new UITapGestureRecognizer(_ =>
            {
                UpdateLastActionTicks();
                overlayView.Hidden = false;
            });
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(this).For(v => v.IsDisliked).To(vm => vm.CurrentVideo.IsDisliked);
            bindingSet.Bind(this).For(v => v.IsSubscribed).To(vm => vm.CurrentVideo.IsSubscribedToUser);
            bindingSet.Bind(this).For(v => v.VideoPlayer).To(vm => vm.CurrentVideo.FullVideoPlayer);
            bindingSet.Bind(this).For(v => v.IsLiked).To(vm => vm.CurrentVideo.IsLiked);
            bindingSet.Bind(this).For(v => v.IsMuted).To(vm => vm.IsMuted).TwoWay();

            bindingSet.Bind(titleLabel).For(v => v.Text).To(vm => vm.CurrentVideo.VideoName);
            bindingSet.Bind(descriptionLabel).For(v => v.Text).To(vm => vm.CurrentVideo.Description);

            bindingSet.Bind(profileView).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.CurrentVideo.AvatarUrl);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.CurrentVideo.ProfileShortName);
            bindingSet.Bind(commentsView).For(v => v.BindTap()).To(vm => vm.OpenCommentsCommand);

            bindingSet.Bind(likeView).For(v => v.BindTap()).To(vm => vm.CurrentVideo.LikeCommand);
            bindingSet.Bind(likeView).For(v => v.UserInteractionEnabled).To(vm => vm.CurrentVideo.CanVoteVideo);
            bindingSet.Bind(dislikeView).For(v => v.BindTap()).To(vm => vm.CurrentVideo.DislikeCommand);
            bindingSet.Bind(dislikeView).For(v => v.UserInteractionEnabled).To(vm => vm.CurrentVideo.CanVoteVideo);
            bindingSet.Bind(likeLabel).For(v => v.Text).To(vm => vm.NumberOfLikesPresentation);
            bindingSet.Bind(dislikeLabel).For(v => v.Text).To(vm => vm.NumberOfDislikesPresentation);

            bindingSet.Bind(commentsLabel).For(v => v.Text).To(vm => vm.NumberOfCommentsPresentation);
            bindingSet.Bind(shareButton).For(v => v.BindTouchUpInside()).To(vm => vm.ShareCommand);
            bindingSet.Bind(this).For(v => v.Interaction).To(vm => vm.Interaction);

            bindingSet.Apply();
        }

        protected override void SetCommonStyles()
        {
            base.SetCommonStyles();

            profileImageView.ClipsToBounds = true;
            profileImageView.Layer.CornerRadius = 20f;
            profileImageView.Layer.BorderColor = Theme.Color.White.CGColor;
            profileImageView.Layer.BorderWidth = 1f;

            watchProgressControl.Layer.CornerRadius = 5f;
            View.BackgroundColor = UIColor.Black;
        }

        private void Initialize()
        {
            InitializePlayer();

            UpdateLastActionTicks();
            Task.Run(HideOverlayAsync);

            VideoPlayer.Play();
        }

        private void OnInteractionRequested(object sender, EventArgs e)
        {
            if (_player.MediaPlayer.State != VLCState.Paused)
            {
                VideoPlayer.Pause();
            }

            UpdateLastActionTicks();
        }

        private void InitializePlayer()
        {
            if (_player?.MediaPlayer != null)
            {
                _player.MediaPlayer.Pause();
                _player.MediaPlayer.Position = 0.0f;
            }

            _player?.RemoveGestureRecognizer(_tapGestureRecognizer);
            _player?.RemoveFromSuperview();

            _player = VideoPlayer.GetNativePlayer() as VideoView;
            PlayerMutedChanged();

            _onEndReachedSubscription?.Dispose();
            _onEndReachedSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>(OnEndReached,
                 (wrapper, handler) => wrapper.EndReached += handler,
                 (wrapper, handler) => wrapper.Playing -= handler);

            _onTimeChangedSubscription?.Dispose();
            _onTimeChangedSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, MediaPlayerTimeChangedEventArgs>(OnTimeChanged,
                 (wrapper, handler) => wrapper.TimeChanged += handler,
                 (wrapper, handler) => wrapper.TimeChanged -= handler);

            _onMutedChangedSubscription?.Dispose();
            _onMutedChangedSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>((_ , __) => PlayerMutedChanged(),
                 (wrapper, handler) => wrapper.Muted += handler,
                 (wrapper, handler) => wrapper.Muted -= handler);

            _onBufferingSubscription?.Dispose();
            _onBufferingSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, MediaPlayerBufferingEventArgs>((_, __) => PlayerBufferingChanged(),
                 (wrapper, handler) => wrapper.Buffering += handler,
                 (wrapper, handler) => wrapper.Buffering -= handler);

            _onPausedSubscription?.Dispose();
            _onPausedSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>((_, __) => PlayerTimeControlStatusChanged(),
                 (wrapper, handler) => wrapper.Paused += handler,
                 (wrapper, handler) => wrapper.Paused -= handler);

            _onPlayingSubscription?.Dispose();
            _onPlayingSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>((_, __) => PlayerTimeControlStatusChanged(),
                 (wrapper, handler) => wrapper.Playing += handler,
                 (wrapper, handler) => wrapper.Playing -= handler);

            _onStoppedSubscription?.Dispose();
            _onStoppedSubscription = _player.MediaPlayer.SubscribeToEvent<LibVLCSharp.Shared.MediaPlayer, EventArgs>((_, __) => PlayerTimeControlStatusChanged(),
                 (wrapper, handler) => wrapper.Stopped += handler,
                 (wrapper, handler) => wrapper.Stopped -= handler);

            View.AddSubview(_player);
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _player.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                _player.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _player.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _player.BottomAnchor.ConstraintEqualTo(View.BottomAnchor)
            });

            _player.AddGestureRecognizer(_tapGestureRecognizer);

            View.BringSubviewToFront(overlayView);
        }

        private void UpdateLastActionTicks()
        {
            _lastActionTicks = DateTime.Now.Ticks;
        }

        private async Task HideOverlayAsync()
        {
            while (true)
            {
                if (GetHideOverlay() && (DateTime.Now.Ticks - _lastActionTicks >= ThreeSecondsTicks))
                {
                    InvokeOnMainThread(() => overlayView.Hidden = true);
                }
                else
                {
                    await Task.Delay(200);
                }
            }
        }

        private bool GetHideOverlay()
        {
            var hideOverlay = true;
            InvokeOnMainThread(() => hideOverlay = !overlayView.Hidden
                                                && _player.MediaPlayer.State != VLCState.Paused);
            return hideOverlay;
        }

        private void OnEndReached(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                _player.MediaPlayer.Position = 0.1f;
            });
        }

        private void OnTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                if (IsPlayerInvalid)
                {
                    return;
                }

                var time = GetTextRepresentation(_player.MediaPlayer.Time);
                var duration = GetTextRepresentation(_player.MediaPlayer.Media.Duration);
                timeLabel.Text = $"{time} / {duration}";

                UpdateTimePassedWidthConstraint();
            });
        }

        private string GetTextRepresentation(long timeInMs)
        {
            var totalSeconds = timeInMs == 0 ? 0 : (int) Math.Ceiling(timeInMs / 1000d);
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;

            return $"{minutes}:{seconds:00}";
        }

        private void UpdateTimePassedWidthConstraint()
        {
            InvokeOnMainThread(() =>
            {
                if (IsPlayerInvalid)
                {
                    return;
                }

                var ratio = _player.MediaPlayer.Time / 1000f / (_player.MediaPlayer.Media.Duration / 1000f);
                var width = ratio * progressView.Frame.Width;
                watchProgressViewWidthConstraint.Constant = width;
            });
        }

        private void PlayerMutedChanged()
        {
            InvokeOnMainThread(() =>
            {
                var imageName = _player.MediaPlayer.Mute ? "ic_sound_muted" : "ic_mute";
                muteButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
            });
        }

        private void PlayerTimeControlStatusChanged()
        {
            InvokeOnMainThread(() =>
            {
                var imageName = _player.MediaPlayer.State == VLCState.Paused
                    ? "ic_play"
                    : "ic_pause";

                playButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
            });
        }

        private void PlayerBufferingChanged()
        {
            InvokeOnMainThread(() =>
            {
                if (IsPlayerInvalid)
                {
                    return;
                }

                var seconds = _player.MediaPlayer.NetworkCaching / 1000;
                var ratio = seconds / _player.MediaPlayer.Media.Duration / 1000;
                var width = ratio * progressView.Frame.Width;
                loadProgressViewWidthConstraint.Constant = width;
            });
        }

        private void PlayButtonTap()
        {
            if (_player.MediaPlayer.State == VLCState.Paused)
            {
                VideoPlayer.Play();
            }
            else
            {
                VideoPlayer.Pause();
            }

            UpdateLastActionTicks();
        }

        private void WatchProgressControlContainerPan(UIPanGestureRecognizer recognizer)
        {
            var point = recognizer.LocationInView(overlayView);
            var newWidth = point.X - watchProgressView.Frame.X;
            var ratio = newWidth / progressView.Frame.Width;
            _player.MediaPlayer.Position = (float)ratio;

            UpdateLastActionTicks();
        }

        private void MuteButtonTap()
        {
            IsMuted = !IsMuted;
            IsMutedChanged?.Invoke(this, EventArgs.Empty);

            UpdateLastActionTicks();
        }

        private void Close()
        {
            if (_interaction != null)
            {
                _interaction.Requested -= OnInteractionRequested;
            }

            if (VideoPlayer != null)
            {
                VideoPlayer.Stop();
            }

            _onEndReachedSubscription?.Dispose();
            _onTimeChangedSubscription?.Dispose();
            _onBufferingSubscription?.Dispose();
            _onPausedSubscription?.Dispose();
            _onPlayingSubscription?.Dispose();
            _onStoppedSubscription?.Dispose();
            _onMutedChangedSubscription?.Dispose();

            ViewModel.CloseCommand.Execute(null);
            ViewModel.ViewDestroy(true);
        }

        private void ViewPan(UIPanGestureRecognizer recognizer)
        {
            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    _oldY = recognizer.LocationInView(View).Y;
                    _firstX = recognizer.LocationInView(View).X;
                    break;

                case UIGestureRecognizerState.Changed:
                    _isSwipeTriggered = true;
                    RecalculateTranslation(recognizer);
                    _lastX = recognizer.LocationInView(View).X;
                    break;

                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Failed:
                    AnimateTranslation();
                    CanExecuteDirectedSwipes(View);
                    break;
            }
        }

        private void CanExecuteDirectedSwipes(UIView view)
        {
            if (!_isSwipeTriggered)
            {
                return;
            }

            if (_firstX < _lastX && _lastX - _firstX >= view.Frame.Width / 4)
            {
                ViewModel.MovePreviousCommand.Execute();
                ResetSwipe();
                return;
            }

            if (_lastX < _firstX && _firstX - _lastX >= view.Frame.Width / 4)
            {
                ViewModel.MoveNextCommand.Execute();
                ResetSwipe();
                return;
            }
        }

        private void ResetSwipe()
        {
            _lastX = 0;
            _firstX = 0;
            _isSwipeTriggered = false;
        }

        private void AnimateTranslation()
        {
            if (View.Frame.Y < View.Frame.Height / 4f)
            {
                UIView.Animate(AnimationDuration, () => View.Frame = new CGRect(0f, 0f, View.Frame.Width, View.Frame.Height));
                return;
            }

            UIView.AnimateNotify(AnimationDuration, 0, UIViewAnimationOptions.CurveEaseOut,
                () =>
                {
                    View.Frame = new CGRect(0f, View.Frame.Height, View.Frame.Width, View.Frame.Height);
                },
                isFinished =>
                {
                    if (isFinished)
                    {
                        Close();
                    }
                });
        }

        private void RecalculateTranslation(UIPanGestureRecognizer recognizer)
        {
            var newY = recognizer.LocationInView(View).Y;
            var differenceY = newY - _oldY;

            var y = View.Frame.Y + differenceY;
            if (y < 0)
            {
                View.Frame = new CGRect(0f, 0f, View.Frame.Width, View.Frame.Height);
                _oldY = newY;
            }
            else
            {
                View.Frame = new CGRect(0f, y, View.Frame.Width, View.Frame.Height);
                _oldY = newY - differenceY;
            }

            recognizer.SetTranslation(CGPoint.Empty, View);
        }

        [Export(nameof(WillResignActive))]
        private void WillResignActive()
        {
            if (_player is null)
            {
                return;
            }

            _wasPlaying = _player.MediaPlayer.State == VLCState.Playing;
            VideoPlayer?.Pause();
        }

        [Export(nameof(DidBecomeActive))]
        private void DidBecomeActive()
        {
            if (_wasPlaying)
            {
                VideoPlayer?.Play();
            }
        }
    }
}