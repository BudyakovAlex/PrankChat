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
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using ObjCRuntime;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Video
{
    //TODO: Tech debt, move controls into XIB control, will provide it in other iteration
    [MvxModalPresentation]
    public partial class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private const string PlayerTimeControlStatusKey = "timeControlStatus";
        private const string PlayerItemLoadedTimeRangesKey = "loadedTimeRanges";
        private const string PlayerMutedKey = "muted";

        private const int ThreeSecondsTicks = 30_000_000;

        private string _videoUrl;
        private bool _wasPlaying;
        private long _lastActionTicks;

        private NSObject _playerPerdiodicTimeObserver;
        private AVPlayer _player;
        private NSObject _playToEndObserver;
        private AVPlayerViewController _controller;

        private UIView _overlayView;
        private UIButton _playButton;
        private UIButton _muteButton;
        private UIButton _closeButton;
        private UIView _progressView;
        private UIView _loadProgressView;
        private UIView _watchProgressView;
        private UIView _watchProgressControl;
        private UILabel _timeLabel;

        private UILabel _titleLabel;
        private UILabel _descriptionLabel;

        private NSLayoutConstraint _timePassedWidthConstraint;
        private NSLayoutConstraint _loadProgressViewWidthConstraint;

        public string VideoUrl
        {
            get => _videoUrl;
            set
            {
                _videoUrl = value;
                Initialize();
            }
        }

        private bool IsPlayerInvalid => _player?.CurrentItem == null || _player.CurrentItem.Duration.IsIndefinite;

        public override void ViewDidLoad()
        {
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
            InitializeOverlayView();
            InitializePlayButton();
            InitializeMuteButton();
            InitializeCloseButton();
            InitializeVideoInfoLabels();
            InitializeProgressView();
            InitializeLoadProgressView();
            InitializeWatchProgressView();
            InitializeWatchProgressControl();
            InitializeTimeLabel();
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(this)
                      .For(v => v.VideoUrl)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(_titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.VideoName);
            bindingSet.Bind(_descriptionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Apply();
        }

        private void Initialize()
        {
            InitializePlayer();
         
            UpdateLastActionTicks();
            Task.Run(HideOverlayAsync);

            _player.Play();
        }

        private void InitializePlayer()
        {
            var url = new NSUrl(_videoUrl);
            var playerItem = new AVPlayerItem(url);
            _player = new AVPlayer(playerItem);

            if (_playToEndObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_playToEndObserver);
            }

            _playToEndObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, OnPlayerPlayedToEnd, playerItem);

            _playerPerdiodicTimeObserver = _player.AddPeriodicTimeObserver(new CMTime(1, 2), DispatchQueue.MainQueue, PlayerTimeChanged);
            _player.AddObserver(this, PlayerTimeControlStatusKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            _player.AddObserver(this, PlayerMutedKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            _player.CurrentItem.AddObserver(this, PlayerItemLoadedTimeRangesKey, NSKeyValueObservingOptions.New, IntPtr.Zero);

            _controller = new AVPlayerViewController();
            _controller.Player = _player;

            _controller.ShowsPlaybackControls = false;
            _controller.View.AddGestureRecognizer(new UITapGestureRecognizer(_ =>
            {
                UpdateLastActionTicks();
                _overlayView.Hidden = false;
            }));

            AddChildViewController(_controller);
            View.AddSubview(_controller.View);
            _controller.View.Frame = View.Frame;
            _controller.DidMoveToParentViewController(this);

            View.BringSubviewToFront(_overlayView);
        }

        private void InitializeOverlayView()
        {
            _overlayView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Black,
                Alpha = 0.8f,
                Hidden = true
            };

            _overlayView.AddGestureRecognizer(new UITapGestureRecognizer(_ => _overlayView.Hidden = true));

            View.AddSubview(_overlayView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _overlayView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _overlayView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                _overlayView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _overlayView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
            });
        }

        private void InitializePlayButton()
        {
            _playButton = new UIButton();
            _playButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _playButton.ContentMode = UIViewContentMode.Center;
            _playButton.AddGestureRecognizer(new UITapGestureRecognizer(PlayButtonTap));
            _playButton.SetImage(UIImage.FromBundle("ic_pause"), UIControlState.Normal);

            _overlayView.AddSubview(_playButton);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _playButton.LeadingAnchor.ConstraintEqualTo(_overlayView.SafeAreaLayoutGuide.LeadingAnchor),
                _playButton.BottomAnchor.ConstraintEqualTo(_overlayView.BottomAnchor, -20f),
                _playButton.HeightAnchor.ConstraintEqualTo(52f),
                _playButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeMuteButton()
        {
            _muteButton = new UIButton();
            _muteButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _muteButton.ContentMode = UIViewContentMode.Center;
            _muteButton.AddGestureRecognizer(new UITapGestureRecognizer(MuteButtonTap));
            _muteButton.SetImage(UIImage.FromBundle("ic_mute"), UIControlState.Normal);

            _overlayView.AddSubview(_muteButton);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _muteButton.TrailingAnchor.ConstraintEqualTo(_overlayView.SafeAreaLayoutGuide.TrailingAnchor),
                _muteButton.BottomAnchor.ConstraintEqualTo(_overlayView.BottomAnchor, -20f),
                _muteButton.HeightAnchor.ConstraintEqualTo(52f),
                _muteButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeCloseButton()
        {
            _closeButton = new UIButton();
            _closeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _closeButton.ContentMode = UIViewContentMode.Center;
            _closeButton.AddGestureRecognizer(new UITapGestureRecognizer(CloseButtonTap));
            _closeButton.SetImage(UIImage.FromBundle("ic_back"), UIControlState.Normal);

            _overlayView.AddSubview(_closeButton);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _closeButton.TopAnchor.ConstraintEqualTo(_overlayView.SafeAreaLayoutGuide.TopAnchor),
                _closeButton.LeadingAnchor.ConstraintEqualTo(_overlayView.SafeAreaLayoutGuide.LeadingAnchor),
                _closeButton.HeightAnchor.ConstraintEqualTo(52f),
                _closeButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeProgressView()
        {
            _progressView = new UIView();
            _progressView.TranslatesAutoresizingMaskIntoConstraints = false;
            _progressView.BackgroundColor = UIColor.White;
            _progressView.Alpha = 0.5f;

            _overlayView.AddSubview(_progressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _progressView.LeadingAnchor.ConstraintEqualTo(_playButton.TrailingAnchor),
                _progressView.TrailingAnchor.ConstraintEqualTo(_muteButton.LeadingAnchor),
                _progressView.CenterYAnchor.ConstraintEqualTo(_muteButton.CenterYAnchor),
                _progressView.HeightAnchor.ConstraintEqualTo(2f),
            });
        }

        private void InitializeLoadProgressView()
        {
            _loadProgressView = new UIView();
            _loadProgressView.TranslatesAutoresizingMaskIntoConstraints = false;
            _loadProgressView.BackgroundColor = UIColor.White;
            _loadProgressView.Alpha = 0.5f;

            _overlayView.AddSubview(_loadProgressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _loadProgressView.LeadingAnchor.ConstraintEqualTo(_progressView.LeadingAnchor),
                _loadProgressView.TopAnchor.ConstraintEqualTo(_progressView.TopAnchor),
                _loadProgressView.BottomAnchor.ConstraintEqualTo(_progressView.BottomAnchor),
                _loadProgressViewWidthConstraint = _loadProgressView.WidthAnchor.ConstraintEqualTo(0f)
            });
        }

        private void InitializeWatchProgressView()
        {
            _watchProgressView = new UIView();
            _watchProgressView.TranslatesAutoresizingMaskIntoConstraints = false;
            _watchProgressView.BackgroundColor = Theme.Color.Accent;

            _overlayView.AddSubview(_watchProgressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _watchProgressView.LeadingAnchor.ConstraintEqualTo(_progressView.LeadingAnchor),
                _watchProgressView.TopAnchor.ConstraintEqualTo(_progressView.TopAnchor),
                _watchProgressView.BottomAnchor.ConstraintEqualTo(_progressView.BottomAnchor),
                _timePassedWidthConstraint = _watchProgressView.WidthAnchor.ConstraintEqualTo(0f)
            });
        }

        private void InitializeWatchProgressControl()
        {
            _watchProgressControl = new UIView();
            _watchProgressControl.TranslatesAutoresizingMaskIntoConstraints = false;
            _watchProgressControl.BackgroundColor = Theme.Color.Accent;
            _watchProgressControl.Layer.CornerRadius = 5f;
            _watchProgressControl.AddGestureRecognizer(new UIPanGestureRecognizer(WatchProgressControlPan));

            _overlayView.AddSubview(_watchProgressControl);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _watchProgressControl.TrailingAnchor.ConstraintEqualTo(_watchProgressView.TrailingAnchor, 5f),
                _watchProgressControl.CenterYAnchor.ConstraintEqualTo(_watchProgressView.CenterYAnchor),
                _watchProgressControl.WidthAnchor.ConstraintEqualTo(10f),
                _watchProgressControl.HeightAnchor.ConstraintEqualTo(10f)
            });
        }

        private void InitializeTimeLabel()
        {
            _timeLabel = new UILabel();
            _timeLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            _timeLabel.TextColor = UIColor.White;
            _timeLabel.Font = Theme.Font.MediumOfSize(12f);
            _timeLabel.Text = "0:00";

            _overlayView.AddSubview(_timeLabel);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                _timeLabel.LeadingAnchor.ConstraintEqualTo(_progressView.LeadingAnchor),
                _timeLabel.BottomAnchor.ConstraintEqualTo(_progressView.BottomAnchor, -11f)
            });
        }

        private void InitializeVideoInfoLabels()
        {
            _titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.White,
                Font = Theme.Font.MediumOfSize(20f),
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 2
            };

            _descriptionLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.White,
                Font = Theme.Font.MediumOfSize(14f),
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0
            };

            _overlayView.AddSubviews(_titleLabel, _descriptionLabel);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _titleLabel.TopAnchor.ConstraintEqualTo(_closeButton.BottomAnchor, 13f),
                _titleLabel.LeadingAnchor.ConstraintEqualTo(_overlayView.LeadingAnchor, 13f),
                _titleLabel.TrailingAnchor.ConstraintEqualTo(_overlayView.TrailingAnchor, -13f),

                _descriptionLabel.TopAnchor.ConstraintEqualTo(_titleLabel.BottomAnchor, 18f),
                _descriptionLabel.LeadingAnchor.ConstraintEqualTo(_overlayView.LeadingAnchor, 13f),
                _descriptionLabel.TrailingAnchor.ConstraintEqualTo(_overlayView.TrailingAnchor, -64f),
            });
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
                    InvokeOnMainThread(() => _overlayView.Hidden = true);
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
            InvokeOnMainThread(() => hideOverlay = !_overlayView.Hidden
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
            _timeLabel.Text = $"{time} / {duration}";

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
            var width = (nfloat) ratio * _progressView.Frame.Width;
            _timePassedWidthConstraint.Constant = width;
        }

        private void PlayerMutedChanged()
        {
            var imageName = _player.Muted ? "ic_sound_muted" : "ic_mute";
            _muteButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerTimeControlStatusChanged()
        {
            var imageName = _player.TimeControlStatus == AVPlayerTimeControlStatus.Paused
                ? "ic_play"
                : "ic_pause";

            _playButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerItemLoadedTimeRangesChanged()
        {
            if (IsPlayerInvalid)
            {
                return;
            }

            var value = _player.CurrentItem.LoadedTimeRanges.Last();
            var seconds = value.CMTimeRangeValue.Start.Seconds + value.CMTimeRangeValue.Duration.Seconds;
            var ratio = seconds / _player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ratio * _progressView.Frame.Width;
            _loadProgressViewWidthConstraint.Constant = width;
        }

        private void PlayButtonTap()
        {
            if (_player.TimeControlStatus == AVPlayerTimeControlStatus.Paused)
            {
                if ((int) _player.CurrentItem.CurrentTime.Seconds == (int) _player.CurrentItem.Duration.Seconds)
                {
                    _player.Seek(new CMTime(0, 1));
                }

                _player.Play();
            }
            else
            {
                _player.Pause();
            }

            UpdateLastActionTicks();
        }

        private void WatchProgressControlPan(UIPanGestureRecognizer recognizer)
        {
            var point = recognizer.LocationInView(_overlayView);
            var newWidth = point.X - _watchProgressView.Frame.X;
            var ratio = newWidth / _progressView.Frame.Width;
            var value = ratio * _player.CurrentItem.Duration.Seconds;
            _player.Seek(new CMTime((long) value, 1));

            UpdateLastActionTicks();
        }

        private void MuteButtonTap()
        {
            _player.Muted = !_player.Muted;
            UpdateLastActionTicks();
        }

        private void CloseButtonTap()
        {
            if (_controller != null)
            {
                _controller.WillMoveToParentViewController(null);
                _controller.View.RemoveFromSuperview();
                _controller.RemoveFromParentViewController();
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