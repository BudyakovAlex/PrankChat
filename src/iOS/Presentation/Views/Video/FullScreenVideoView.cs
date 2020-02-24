﻿using System;
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

        private string _videoUrl;
        private bool _wasPlaying;
        private long _lastActionTicks;

        private NSObject _playerPerdiodicTimeObserver;
        private AVPlayer _player;
        private NSObject _playToEndObserver;
        private AVPlayerViewController _controller;

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
            overlayView.AddGestureRecognizer(new UITapGestureRecognizer(_ => overlayView.Hidden = true));

            playButton.AddGestureRecognizer(new UITapGestureRecognizer(PlayButtonTap));

            muteButton.AddGestureRecognizer(new UITapGestureRecognizer(MuteButtonTap));

            closeButton.AddGestureRecognizer(new UITapGestureRecognizer(CloseButtonTap));

            watchProgressControl.Layer.CornerRadius = 5f;

            watchProgressControlContainer.AddGestureRecognizer(new UIPanGestureRecognizer(WatchProgressControlContainerPan));
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(this)
                      .For(v => v.VideoUrl)
                      .To(vm => vm.VideoUrl);

            bindingSet.Bind(titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.VideoName);
            bindingSet.Bind(descriptionLabel)
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

            var value = _player.CurrentItem.LoadedTimeRanges.Last();
            var seconds = value.CMTimeRangeValue.Start.Seconds + value.CMTimeRangeValue.Duration.Seconds;
            var ratio = seconds / _player.CurrentItem.Duration.Seconds;
            var width = (nfloat) ratio * progressView.Frame.Width;
            loadProgressViewWidthConstraint.Constant = width;
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