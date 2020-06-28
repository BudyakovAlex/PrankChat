using System;
using System.Linq;
using System.Threading.Tasks;
using AVFoundation;
using AVKit;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.ViewModels;
using ObjCRuntime;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Video
{
    [MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.FullScreen)]
    public partial class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private const string PlayerTimeControlStatusKey = "timeControlStatus";
        private const string PlayerItemLoadedTimeRangesKey = "loadedTimeRanges";
        private const string PlayerMutedKey = "muted";

        private const int ThreeSecondsTicks = 30_000_000;
        private const double AnimationDuration = 0.3d;

        private bool _wasPlaying;
        private long _lastActionTicks;
        private nfloat _oldY;
        private nfloat _lastX;
        private nfloat _firstX;

        private NSObject _playerPerdiodicTimeObserver;
        private AVPlayer _player;
        private NSObject _playToEndObserver;
        private AVPlayerViewController _controller;

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

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            set
            {
                _videoUrl = value;
                if (_videoUrl is null)
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
        private bool _isSwipeTriggered;

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_player is null)
                {
                    return;
                }

                _player.Muted = value;
            }
        }

        private bool IsPlayerInvalid => _player?.CurrentItem == null || _player.CurrentItem.Duration.IsIndefinite;

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
                PlayerItemLoadedTimeRangesChanged();
                UpdateTimePassedWidthConstraint();
            }, null);

            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            switch (keyPath)
            {
                case PlayerItemLoadedTimeRangesKey:
                    PlayerItemLoadedTimeRangesChanged();
                    break;

                case PlayerTimeControlStatusKey:
                    PlayerTimeControlStatusChanged();
                    break;

                case PlayerMutedKey:
                    PlayerMutedChanged();
                    break;
            }
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
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(this).For(v => v.IsDisliked).To(vm => vm.IsDisliked);
            bindingSet.Bind(this).For(v => v.IsSubscribed).To(vm => vm.IsSubscribed);

            bindingSet.Bind(this)
                      .For(v => v.VideoUrl)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.VideoName);
            bindingSet.Bind(descriptionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(profileView)
                      .For(v => v.BindTap())
                      .To(vm => vm.OpenUserProfileCommand);

            bindingSet.Bind(profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl);

            bindingSet.Bind(profileImageView)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortName);

            bindingSet.Bind(commentsView)
                      .For(v => v.BindTap())
                      .To(vm => vm.OpenCommentsCommand);

            bindingSet.Bind(likeView)
                      .For(v => v.BindTap())
                      .To(vm => vm.LikeCommand);

            bindingSet.Bind(likeView)
                      .For(v => v.UserInteractionEnabled)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(dislikeView)
                      .For(v => v.BindTap())
                      .To(vm => vm.DislikeCommand);

            bindingSet.Bind(dislikeView)
                      .For(v => v.UserInteractionEnabled)
                      .To(vm => vm.IsLikeFlowAvailable);

            bindingSet.Bind(this)
                      .For(v => v.IsLiked)
                      .To(vm => vm.IsLiked);

            bindingSet.Bind(this)
                      .For(v => v.IsMuted)
                      .To(vm => vm.IsMuted)
                      .TwoWay();

            bindingSet.Bind(likeLabel)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfLikesPresentation);

            bindingSet.Bind(dislikeLabel)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfDislikesPresentation);

            bindingSet.Bind(commentsLabel)
                      .For(v => v.Text)
                      .To(vm => vm.NumberOfCommentsPresentation);

            bindingSet.Bind(shareButton)
                      .For(v => v.BindTouchUpInside())
                      .To(vm => vm.ShareCommand);

            bindingSet.Bind(this)
                      .For(v => v.Interaction)
                      .To(vm => vm.Interaction);

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
        }

        private void Initialize()
        {
            InitializePlayer();

            UpdateLastActionTicks();
            Task.Run(HideOverlayAsync);

            _player.Play();
        }

        private void OnInteractionRequested(object sender, EventArgs e)
        {
            if (_player.TimeControlStatus != AVPlayerTimeControlStatus.Paused)
            {
                _player.Pause();
            }

            UpdateLastActionTicks();
        }

        private void InitializePlayer()
        {
            var url = new NSUrl(_videoUrl);
            var playerItem = new AVPlayerItem(url);

            if (_player != null)
            {
                _player.RemoveTimeObserver(_playerPerdiodicTimeObserver);
                _player.RemoveObserver(this, PlayerTimeControlStatusKey, IntPtr.Zero);
                _player.RemoveObserver(this, PlayerMutedKey, IntPtr.Zero);
                _player.CurrentItem.RemoveObserver(this, PlayerItemLoadedTimeRangesKey, IntPtr.Zero);

                _player.Pause();
                _player.Dispose();
            }

            _player = new AVPlayer(playerItem)
            {
                Muted = _isMuted
            };

            PlayerMutedChanged();

            if (_playToEndObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_playToEndObserver);
            }

            _playToEndObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, OnPlayerPlayedToEnd, playerItem);

            _playerPerdiodicTimeObserver = _player.AddPeriodicTimeObserver(new CMTime(1, 2), DispatchQueue.MainQueue, PlayerTimeChanged);
            _player.AddObserver(this, PlayerTimeControlStatusKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            _player.AddObserver(this, PlayerMutedKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            _player.CurrentItem.AddObserver(this, PlayerItemLoadedTimeRangesKey, NSKeyValueObservingOptions.New, IntPtr.Zero);

            if (_controller != null)
            {
                _controller.Player = _player;
                return;
            }

            _controller = new AVPlayerViewController
            {
                Player = _player,
                ShowsPlaybackControls = false
            };

            _controller.View.AddGestureRecognizer(new UITapGestureRecognizer(_ =>
            {
                UpdateLastActionTicks();
                overlayView.Hidden = false;
            }));

            AddChildViewController(_controller);
            View.AddSubview(_controller.View);
            _controller.View.Frame = View.Frame;
            _controller.DidMoveToParentViewController(this);

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
                                                && _player.TimeControlStatus != AVPlayerTimeControlStatus.Paused);
            return hideOverlay;
        }

        private void OnPlayerPlayedToEnd(NSNotification notification)
        {
            _player.Seek(CMTime.Zero);
            _player.Play();

            notification.Dispose();
        }

        private void PlayerTimeChanged(CMTime _)
        {
            if (IsPlayerInvalid)
            {
                return;
            }

            var time = GetTextRepresentation(_player.CurrentItem.CurrentTime);
            var duration = GetTextRepresentation(_player.CurrentItem.Duration);
            timeLabel.Text = $"{time} / {duration}";

            UpdateTimePassedWidthConstraint();
        }

        private string GetTextRepresentation(CMTime time)
        {
            var totalSeconds = (int) Math.Ceiling(time.Seconds);
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;

            return $"{minutes}:{seconds.ToString("00")}";
        }

        private void UpdateTimePassedWidthConstraint()
        {
            if (IsPlayerInvalid)
            {
                return;
            }

            var ratio = _player.CurrentItem.CurrentTime.Seconds / _player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ratio * progressView.Frame.Width;
            watchProgressViewWidthConstraint.Constant = width;
        }

        private void PlayerMutedChanged()
        {
            var imageName = _player.Muted ? "ic_sound_muted" : "ic_mute";
            muteButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerTimeControlStatusChanged()
        {
            var imageName = _player.TimeControlStatus == AVPlayerTimeControlStatus.Paused
                ? "ic_play"
                : "ic_pause";

            playButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerItemLoadedTimeRangesChanged()
        {
            if (IsPlayerInvalid)
            {
                return;
            }

            var value = _player.CurrentItem.LoadedTimeRanges.LastOrDefault();
            if (value == null)
                return;

            var seconds = value.CMTimeRangeValue.Start.Seconds + value.CMTimeRangeValue.Duration.Seconds;
            var ratio = seconds / _player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ratio * progressView.Frame.Width;
            loadProgressViewWidthConstraint.Constant = width;
        }

        private void PlayButtonTap()
        {
            if (_player.TimeControlStatus == AVPlayerTimeControlStatus.Paused)
            {
                _player.Play();
            }
            else
            {
                _player.Pause();
            }

            UpdateLastActionTicks();
        }

        private void WatchProgressControlContainerPan(UIPanGestureRecognizer recognizer)
        {
            var point = recognizer.LocationInView(overlayView);
            var newWidth = point.X - watchProgressView.Frame.X;
            var ratio = newWidth / progressView.Frame.Width;
            var value = ratio * _player.CurrentItem.Duration.Seconds;
            _player.Seek(new CMTime((long) value, 1));

            UpdateLastActionTicks();
        }

        private void MuteButtonTap()
        {
            IsMuted = !_player.Muted;
            IsMutedChanged?.Invoke(this, EventArgs.Empty);

            UpdateLastActionTicks();
        }

        private void Close()
        {
            if (_controller != null)
            {
                _controller.WillMoveToParentViewController(null);
                _controller.View.RemoveFromSuperview();
                _controller.RemoveFromParentViewController();
            }

            if (_interaction != null)
            {
                _interaction.Requested -= OnInteractionRequested;
            }

            if (_player != null)
            {
                _player.RemoveTimeObserver(_playerPerdiodicTimeObserver);
                _player.RemoveObserver(this, PlayerTimeControlStatusKey, IntPtr.Zero);
                _player.RemoveObserver(this, PlayerMutedKey, IntPtr.Zero);
                _player.CurrentItem.RemoveObserver(this, PlayerItemLoadedTimeRangesKey, IntPtr.Zero);

                _player.Pause();
                _player.Dispose();
            }

            if (_playToEndObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_playToEndObserver);
            }

            NSNotificationCenter.DefaultCenter.RemoveObserver(this);

            ViewModel.GoBackCommand.ExecuteAsync();
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
            _wasPlaying = _player.TimeControlStatus == AVPlayerTimeControlStatus.Playing;
            _player.Pause();
        }

        [Export(nameof(DidBecomeActive))]
        private void DidBecomeActive()
        {
            if (_wasPlaying)
            {
                _player.Play();
            }
        }
    }
}