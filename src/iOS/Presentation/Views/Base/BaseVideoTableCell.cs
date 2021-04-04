using AVFoundation;
using AVKit;
using CoreAnimation;
using CoreGraphics;
using FFImageLoading.Cross;
using Foundation;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.iOS.AppTheme;
using System;
using UIKit;
using Xamarin.Essentials;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseVideoTableCell : BaseTableCell
    {
        private CAGradientLayer _gradientLayer;
        private AVPlayerViewController _controller;
        private CALayer _backgroundLayer;

        protected BaseVideoTableCell(IntPtr handle)
            : base(handle)
        {
        }

        public BaseVideoItemViewModel ViewModel => BindingContext.DataContext as BaseVideoItemViewModel;

        public abstract MvxCachedImageView StubImageView { get; }
        public abstract UIActivityIndicatorView LoadingActivityIndicator { get; }
        protected abstract UIView VideoView { get; }
        protected abstract UIView RootProcessingBackgroundView { get; }
        protected abstract UILabel ProcessingLabel { get; }
        protected abstract UIView ProcessingBackgroundView { get; }
        protected abstract UIActivityIndicatorView ProcessingActivityIndicator { get; }

        private bool _canShowStub = true;
        public bool CanShowStub
        {
            get => _canShowStub;
            set
            {
                _canShowStub = value;
                if (!value)
                {
                    StubImageView.Hidden = true;
                    LoadingActivityIndicator.Hidden = true;
                }

                ProcessingActivityIndicator.StartAnimating();
            }
        }

        private IVideoPlayer _videoPlayer;
        public IVideoPlayer VideoPlayer
        {
            get => _videoPlayer;
            set
            {
                _videoPlayer = value;
                SetPlayerState();
            }
        }

        public override void PrepareForReuse()
        {
            StubImageView.Image = null;
     
            ShowStub();
            StopVideo();

            base.PrepareForReuse();
        }

        protected override void Dispose(bool disposing)
        {
            StopVideo();

            base.Dispose(disposing);
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            VideoView.SetPreviewStyle();

            RootProcessingBackgroundView.BackgroundColor = UIColor.Clear;
            _gradientLayer = new CAGradientLayer
            {
                Colors = new[] {  Theme.Color.CompetitionPhaseNewPrimary.CGColor, Theme.Color.CompetitionPhaseNewSecondary.CGColor },
                CornerRadius = 10,
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 0f)
            };

            _backgroundLayer = new CALayer() { BackgroundColor = UIColor.Black.CGColor };

            VideoView.Layer.AddSublayer(_backgroundLayer);

            ProcessingLabel.Text = Resources.Processing_Video;
            RootProcessingBackgroundView.Layer.InsertSublayer(_gradientLayer, 0);
            ProcessingBackgroundView.Layer.CornerRadius = 8;

            LoadingActivityIndicator.Hidden = true;
            StubImageView.Hidden = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _gradientLayer.Frame = RootProcessingBackgroundView.Bounds;
            _backgroundLayer.Frame = RootProcessingBackgroundView.Bounds;

            if (_controller != null)
            {
                _controller.View.Frame = VideoView.Bounds;
            }
        }

        public CGRect GetVideoBounds(UITableView tableView) =>
            VideoView.ConvertRectToView(VideoView.Bounds, tableView);

        public void ShowStub()
        {
            if (!_canShowStub)
            {
                return;
            }

            StubImageView.Hidden = false;
            LoadingActivityIndicator.Hidden = true;
        }

        private void HideStubs()
        {
            StubImageView.Hidden = true;
            LoadingActivityIndicator.Hidden = true;
        }

        private void StopVideo()
        {
            VideoPlayer?.Stop();
        }

        private void SetPlayerState()
        {
            if (VideoPlayer?.GetNativePlayer() is AVPlayer player)
            {
                _controller?.View.RemoveFromSuperview();
                _controller = new AVPlayerViewController
                {
                    Player = player,
                    ShowsPlaybackControls = false
                };

                VideoView.AddSubview(_controller.View);
                _controller.View.Frame = VideoView.Bounds;
                
                _videoPlayer.ReadyToPlayAction = HideStubs;
            }
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<BaseVideoTableCell, BaseVideoItemViewModel>();

            bindingSet.Bind(this).For(v => v.VideoPlayer).To(vm => vm.PreviewVideoPlayer);
        }
    }

    public abstract class BaseVideoTableCell<TCell, TViewModel> : BaseVideoTableCell
        where TCell : BaseVideoTableCell
		where TViewModel : BaseVideoItemViewModel
    {
        protected BaseVideoTableCell(IntPtr handle)
            : base(handle)
        {
        }

		static BaseVideoTableCell()
		{
			CellId = typeof(TCell).Name;
			Nib = UINib.FromName(CellId, NSBundle.MainBundle);
		}

		public static string CellId { get; }

		public static UINib Nib { get; }

		public new TViewModel ViewModel => BindingContext.DataContext as TViewModel;
    }
}