using System;
using System.Threading;
using AVFoundation;
using AVKit;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public partial class PublicationItemCell : BaseTableCell<PublicationItemCell, PublicationItemViewModel>
    {
        private const int VideoRepeatDelayInSeconds = 10;
        private AVPlayerViewController _avPlayerViewController;
        private AVQueuePlayer _avPlayer;
        private AVPlayerLooper _avPlayerLooper;

        static PublicationItemCell()
        {
            EstimatedHeight = 334;
        }

        protected PublicationItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void PlayVideo()
        {
            _avPlayer.Play();
        }

        public void StopVideo()
        {
            _avPlayer.Seek(new CMTime(0, 1));
            _avPlayer.Pause();
        }

        public void PrerollVideo(string uri)
        {
            if (_avPlayer == null)
                _avPlayer = new AVQueuePlayer(new[] { new AVPlayerItem(new NSUrl(uri)) });

            // Initialize looper for player that will repeat first 10 seconds of video in a loop.
            if (_avPlayerLooper == null)
                _avPlayerLooper =
                    new AVPlayerLooper(_avPlayer, _avPlayer.CurrentItem,
                        new CMTimeRange {
                            Start = new CMTime(0, 1),
                            Duration = new CMTime(VideoRepeatDelayInSeconds, 1)});
        }

        public CGRect VideoBounds => videoView.Bounds;

        public override void PrepareForReuse()
        {
            StopVideo();
            base.PrepareForReuse();
        }

        protected override void Dispose(bool disposing)
        {
            _avPlayer.Pause();
            _avPlayer.Dispose();
            base.Dispose(disposing);
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            videoView.SetPreviewStyle();
            profileNameLabel.SetMainTitleStyle();
            publicationInfoLabel.SetSmallSubtitleStyle();
            videoNameLabel.SetTitleStyle();
            likeLabel.SetSmallTitleStyle();
            shareLabel.SetSmallTitleStyle();
            shareLabel.Text = Resources.Share;

            InitializeVideoControl();
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<PublicationItemCell, PublicationItemViewModel>();

            set.Bind(profileImage)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImage)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);
                
            set.Bind(profileImage)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            //set.Bind(videoImage)
            //    .For(v => v.ImagePath)
            //    .To(vm => vm.VideoUrl)
            //    .Mode(MvxBindingMode.OneTime);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(videoNameLabel)
                .To(vm => vm.VideoName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(publicationInfoLabel)
                .To(vm => vm.VideoInformationText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(likeLabel)
                .To(vm => vm.NumberOfLikesText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(moreButton)
                .To(vm => vm.OpenSettingsCommand)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(shareButton)
                .To(vm => vm.ShareCommand)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
        }

        private void InitializeVideoControl()
        {
            if (_avPlayer == null || _avPlayerLooper == null)
                throw new ArgumentException("Player hasn't been initialized, please, check Preroll method called before.");

            _avPlayer.Muted = false;
            _avPlayerViewController = new AVPlayerViewController();
            _avPlayerViewController.Player = _avPlayer;
            _avPlayerViewController.View.Frame = new CGRect(0, 0, videoView.Frame.Width, videoView.Frame.Height);
            _avPlayerViewController.ShowsPlaybackControls = false;
            _avPlayerViewController.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
            videoView.Add(_avPlayerViewController.View);
        }
    }
}

