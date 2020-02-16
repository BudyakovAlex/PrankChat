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
    [MvxModalPresentation]
    public partial class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private const string PlayerTimeControlStatusKey = "timeControlStatus";
        private const string PlayerItemLoadedTimeRangesKey = "loadedTimeRanges";
        private const string PlayerMutedKey = "muted";

        private const int ThreeSecondsTicks = 30_000_000;

        private string videoUrl;
        private bool wasPlaying;
        private long lastActionTicks;

        private AVPlayer player;
        private AVPlayerViewController controller;

        private UIView overlayView;
        private UIButton playButton;
        private UIButton muteButton;
        private UIButton closeButton;
        private UIView progressView;
        private UIView loadProgressView;
        private UIView watchProgressView;
        private UIView watchProgressControl;
        private UILabel timeLabel;

        private NSLayoutConstraint timePassedWidthConstraint;
        private NSLayoutConstraint loadProgressViewWidthConstraint;

        public string VideoUrl
        {
            get => videoUrl;
            set
            {
                videoUrl = value;
                Initialize();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NSNotificationCenter.DefaultCenter.AddObserver(this, new Selector(nameof(WillResignActive)), UIApplication.WillResignActiveNotification, null);
            NSNotificationCenter.DefaultCenter.AddObserver(this, new Selector(nameof(DidBecomeActive)), UIApplication.DidBecomeActiveNotification, null);
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();
            bindingSet.Bind(this).For(v => v.VideoUrl).To(vm => vm.VideoUrl);
            bindingSet.Apply();
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

        private void Initialize()
        {
            InitializePlayer();
            InitializeOverlayView();
            InitializePlayButton();
            InitializeMuteButton();
            InitializeCloseButton();
            InitializeProgressView();
            InitializeLoadProgressView();
            InitializeWatchProgressView();
            InitializeWatchProgressControl();
            InitializeTimeLabel();

            UpdateLastActionTicks();
            Task.Run(HideOverlayAsync);

            player.Play();
        }

        private void InitializePlayer()
        {
            var url = new NSUrl(videoUrl);
            var playerItem = new AVPlayerItem(url);
            player = new AVPlayer(playerItem);

            player.AddPeriodicTimeObserver(new CMTime(1, 2), DispatchQueue.MainQueue, PlayerTimeChanged);
            player.AddObserver(this, PlayerTimeControlStatusKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            player.AddObserver(this, PlayerMutedKey, NSKeyValueObservingOptions.New, IntPtr.Zero);
            player.CurrentItem.AddObserver(this, PlayerItemLoadedTimeRangesKey, NSKeyValueObservingOptions.New, IntPtr.Zero);

            controller = new AVPlayerViewController();
            controller.Player = player;
            controller.ShowsPlaybackControls = false;
            controller.View.AddGestureRecognizer(new UITapGestureRecognizer(_ =>
            {
                UpdateLastActionTicks();
                overlayView.Hidden = false;
            }));

            AddChildViewController(controller);
            View.AddSubview(controller.View);
            controller.View.Frame = View.Frame;
            controller.DidMoveToParentViewController(this);
        }

        private void InitializeOverlayView()
        {
            overlayView = new UIView();
            overlayView.TranslatesAutoresizingMaskIntoConstraints = false;
            overlayView.BackgroundColor = UIColor.Black;
            overlayView.Alpha = 0.8f;
            overlayView.AddGestureRecognizer(new UITapGestureRecognizer(_ => overlayView.Hidden = true));

            View.AddSubview(overlayView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                overlayView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                overlayView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                overlayView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                overlayView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
            });
        }

        private void InitializePlayButton()
        {
            playButton = new UIButton();
            playButton.TranslatesAutoresizingMaskIntoConstraints = false;
            playButton.ContentMode = UIViewContentMode.Center;
            playButton.AddGestureRecognizer(new UITapGestureRecognizer(PlayButtonTap));
            playButton.SetImage(UIImage.FromBundle("FullscreenVideoPause"), UIControlState.Normal);

            overlayView.AddSubview(playButton);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                playButton.LeadingAnchor.ConstraintEqualTo(overlayView.SafeAreaLayoutGuide.LeadingAnchor),
                playButton.BottomAnchor.ConstraintEqualTo(overlayView.BottomAnchor, -20f),
                playButton.HeightAnchor.ConstraintEqualTo(52f),
                playButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeMuteButton()
        {
            muteButton = new UIButton();
            muteButton.TranslatesAutoresizingMaskIntoConstraints = false;
            muteButton.ContentMode = UIViewContentMode.Center;
            muteButton.AddGestureRecognizer(new UITapGestureRecognizer(MuteButtonTap));
            muteButton.SetImage(UIImage.FromBundle("FullscreenVideoMute"), UIControlState.Normal);

            overlayView.AddSubview(muteButton);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                muteButton.TrailingAnchor.ConstraintEqualTo(overlayView.SafeAreaLayoutGuide.TrailingAnchor),
                muteButton.BottomAnchor.ConstraintEqualTo(overlayView.BottomAnchor, -20f),
                muteButton.HeightAnchor.ConstraintEqualTo(52f),
                muteButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeCloseButton()
        {
            closeButton = new UIButton();
            closeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            closeButton.ContentMode = UIViewContentMode.Center;
            closeButton.AddGestureRecognizer(new UITapGestureRecognizer(CloseButtonTap));
            closeButton.SetImage(UIImage.FromBundle("ic_back"), UIControlState.Normal);

            overlayView.AddSubview(closeButton);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                closeButton.TopAnchor.ConstraintEqualTo(overlayView.SafeAreaLayoutGuide.TopAnchor),
                closeButton.LeadingAnchor.ConstraintEqualTo(overlayView.SafeAreaLayoutGuide.LeadingAnchor),
                closeButton.HeightAnchor.ConstraintEqualTo(52f),
                closeButton.WidthAnchor.ConstraintEqualTo(52f)
            });
        }

        private void InitializeProgressView()
        {
            progressView = new UIView();
            progressView.TranslatesAutoresizingMaskIntoConstraints = false;
            progressView.BackgroundColor = UIColor.White;
            progressView.Alpha = 0.5f;

            overlayView.AddSubview(progressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                progressView.LeadingAnchor.ConstraintEqualTo(playButton.TrailingAnchor),
                progressView.TrailingAnchor.ConstraintEqualTo(muteButton.LeadingAnchor),
                progressView.CenterYAnchor.ConstraintEqualTo(muteButton.CenterYAnchor),
                progressView.HeightAnchor.ConstraintEqualTo(2f),
            });
        }

        private void InitializeLoadProgressView()
        {
            loadProgressView = new UIView();
            loadProgressView.TranslatesAutoresizingMaskIntoConstraints = false;
            loadProgressView.BackgroundColor = UIColor.White;
            loadProgressView.Alpha = 0.5f;

            overlayView.AddSubview(loadProgressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                loadProgressView.LeadingAnchor.ConstraintEqualTo(progressView.LeadingAnchor),
                loadProgressView.TopAnchor.ConstraintEqualTo(progressView.TopAnchor),
                loadProgressView.BottomAnchor.ConstraintEqualTo(progressView.BottomAnchor),
                loadProgressViewWidthConstraint = loadProgressView.WidthAnchor.ConstraintEqualTo(0f)
            });
        }

        private void InitializeWatchProgressView()
        {
            watchProgressView = new UIView();
            watchProgressView.TranslatesAutoresizingMaskIntoConstraints = false;
            watchProgressView.BackgroundColor = Theme.Color.Accent;

            overlayView.AddSubview(watchProgressView);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                watchProgressView.LeadingAnchor.ConstraintEqualTo(progressView.LeadingAnchor),
                watchProgressView.TopAnchor.ConstraintEqualTo(progressView.TopAnchor),
                watchProgressView.BottomAnchor.ConstraintEqualTo(progressView.BottomAnchor),
                timePassedWidthConstraint = watchProgressView.WidthAnchor.ConstraintEqualTo(0f)
            });
        }

        private void InitializeWatchProgressControl()
        {
            watchProgressControl = new UIView();
            watchProgressControl.TranslatesAutoresizingMaskIntoConstraints = false;
            watchProgressControl.BackgroundColor = Theme.Color.Accent;
            watchProgressControl.Layer.CornerRadius = 5f;
            watchProgressControl.AddGestureRecognizer(new UIPanGestureRecognizer(WatchProgressControlPan));

            overlayView.AddSubview(watchProgressControl);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                watchProgressControl.TrailingAnchor.ConstraintEqualTo(watchProgressView.TrailingAnchor, 5f),
                watchProgressControl.CenterYAnchor.ConstraintEqualTo(watchProgressView.CenterYAnchor),
                watchProgressControl.WidthAnchor.ConstraintEqualTo(10f),
                watchProgressControl.HeightAnchor.ConstraintEqualTo(10f)
            });
        }

        private void InitializeTimeLabel()
        {
            timeLabel = new UILabel();
            timeLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            timeLabel.TextColor = UIColor.White;
            timeLabel.Font = Theme.Font.MediumOfSize(12f);
            timeLabel.Text = "0:00";

            overlayView.AddSubview(timeLabel);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                timeLabel.LeadingAnchor.ConstraintEqualTo(progressView.LeadingAnchor),
                timeLabel.BottomAnchor.ConstraintEqualTo(progressView.BottomAnchor, -11f)
            });
        }

        private void UpdateLastActionTicks()
        {
            lastActionTicks = DateTime.Now.Ticks;
        }

        private async Task HideOverlayAsync()
        {
            while (true)
            {
                if (GetHideOverlay() && (DateTime.Now.Ticks - lastActionTicks >= ThreeSecondsTicks))
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
                                                && player.TimeControlStatus != AVPlayerTimeControlStatus.Paused);

            return hideOverlay;
        }

        private void PlayerTimeChanged(CMTime _)
        {
            if (player.CurrentItem == null)
            {
                return;
            }

            var time = GetTextRepresentation(player.CurrentItem.CurrentTime);
            var duration = GetTextRepresentation(player.CurrentItem.Duration);
            timeLabel.Text = $"{time} / {duration}";

            UpdateTimePassedWidthConstraint();
        }

        private string GetTextRepresentation(CMTime time)
        {
            var totalSeconds = (int) Math.Floor(time.Seconds);
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;

            return $"{minutes}:{seconds.ToString("00")}";
        }

        private void UpdateTimePassedWidthConstraint()
        {
            if (player.CurrentItem == null)
            {
                return;
            }

            var ration = player.CurrentItem.CurrentTime.Seconds / player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ration * progressView.Frame.Width;
            timePassedWidthConstraint.Constant = width;
        }

        private void PlayerMutedChanged()
        {
            var imageName = player.Muted ? "ic_sound_muted" : "FullscreenVideoMute";
            muteButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerTimeControlStatusChanged()
        {
            var imageName = player.TimeControlStatus == AVPlayerTimeControlStatus.Paused
                ? "FullscreenVideoPlay"
                : "FullscreenVideoPause";

            playButton.SetImage(UIImage.FromBundle(imageName), UIControlState.Normal);
        }

        private void PlayerItemLoadedTimeRangesChanged()
        {
            if (player.CurrentItem == null)
            {
                return;
            }

            var value = player.CurrentItem.LoadedTimeRanges.Last();
            var seconds = value.CMTimeRangeValue.Start.Seconds + value.CMTimeRangeValue.Duration.Seconds;
            var ratio = seconds / player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ratio * progressView.Frame.Width;
            loadProgressViewWidthConstraint.Constant = width;
        }

        private void PlayButtonTap()
        {
            if (player.TimeControlStatus == AVPlayerTimeControlStatus.Paused)
            {
                if ((int) player.CurrentItem.CurrentTime.Seconds == (int) player.CurrentItem.Duration.Seconds)
                {
                    player.Seek(new CMTime(0, 1));
                }

                player.Play();
            }
            else
            {
                player.Pause();
            }

            UpdateLastActionTicks();
        }

        private void WatchProgressControlPan(UIPanGestureRecognizer recognizer)
        {
            var point = recognizer.LocationInView(overlayView);
            var newWidth = point.X - watchProgressView.Frame.X;
            var ratio = newWidth / progressView.Frame.Width;
            var value = ratio * player.CurrentItem.Duration.Seconds;
            player.Seek(new CMTime((long) value, 1));

            UpdateLastActionTicks();
        }

        private void MuteButtonTap()
        {
            player.Muted = !player.Muted;
            UpdateLastActionTicks();
        }

        private void CloseButtonTap()
        {
            controller.WillMoveToParentViewController(null);
            controller.View.RemoveFromSuperview();
            controller.RemoveFromParentViewController();
            player.Dispose();

            ViewModel.GoBackCommand.ExecuteAsync();
        }

        [Export(nameof(WillResignActive))]
        private void WillResignActive()
        {
            wasPlaying = player.TimeControlStatus == AVPlayerTimeControlStatus.Playing;
            player.Pause();
        }

        [Export(nameof(DidBecomeActive))]
        private void DidBecomeActive()
        {
            if (wasPlaying)
            {
                player.Play();
            }
        }
    }
}