using AVFoundation;
using AVKit;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using FFImageLoading.Cross;
using Foundation;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.iOS.AppTheme;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseVideoTableCell : BaseTableCell
    {
        private const string PlayerStatusObserverKey = "status";

        private NSObject _playerPerdiodicTimeObserver;
        private bool _isObserverRemoved;
        private CAGradientLayer _gradientLayer;

        protected BaseVideoTableCell(IntPtr handle)
            : base(handle)
        {
        }

        public BaseVideoItemViewModel ViewModel => BindingContext.DataContext as BaseVideoItemViewModel;

        public AVPlayerViewController AVPlayerViewControllerInstance { get; private set; }

        public abstract MvxCachedImageView StubImageView { get; }

        protected abstract UIView VideoView { get; }

        protected abstract UIActivityIndicatorView LoadingActivityIndicator { get; }

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
                //_canShowStub = value;
                //if (!value)
                //{
                //    StubImageView.Hidden = true;
                //    LoadingActivityIndicator.Hidden = true;
                //}

                //ProcessingActivityIndicator.StartAnimating();
            }
        }

        //public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        //{
        //    if (keyPath != PlayerStatusObserverKey)
        //    {
        //        return;
        //    }

        //    if (AVPlayerViewControllerInstance.Player != null &&
        //        AVPlayerViewControllerInstance.Player.Status == AVPlayerStatus.ReadyToPlay)
        //    {
        //        LoadingActivityIndicator.Hidden = true;
        //        StubImageView.Hidden = true;
        //    }
        //}

        public override void PrepareForReuse()
        {
            StubImageView.Image = null;

            //ShowStub();

            //if (!_isObserverRemoved)
            //{
            //    AVPlayerViewControllerInstance.Player?.RemoveTimeObserver(_playerPerdiodicTimeObserver);
            //    _playerPerdiodicTimeObserver = null;
            //}

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
            InitializeVideoControl();

            RootProcessingBackgroundView.BackgroundColor = UIColor.Clear;
            _gradientLayer = new CAGradientLayer
            {
                Colors = new[] {  Theme.Color.CompetitionPhaseNewPrimary.CGColor, Theme.Color.CompetitionPhaseNewSecondary.CGColor },
                CornerRadius = 10,
                StartPoint = new CGPoint(0f, 1f),
                EndPoint = new CGPoint(1f, 0f)
            };

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
        }

        public CGRect GetVideoBounds(UITableView tableView) =>
            VideoView.ConvertRectToView(VideoView.Bounds, tableView);

        //public void AddObserverForPeriodicTime()
        //{
        //    LoadingActivityIndicator.Hidden = false;
        //    LoadingActivityIndicator.StartAnimating();

        //    _playerPerdiodicTimeObserver = AVPlayerViewControllerInstance.Player?.AddPeriodicTimeObserver(
        //        new CMTime(1, 2),
        //        DispatchQueue.MainQueue,
        //        PlayerTimeChanged);
        //}

        //public void ShowStub()
        //{
        //    if (!_canShowStub)
        //    {
        //        return;
        //    }

        //    StubImageView.Hidden = false;
        //    LoadingActivityIndicator.Hidden = true;
        //}

        //private void PlayerTimeChanged(CMTime obj)
        //{
        //    if (obj.Value > 0)
        //    {
        //        _isObserverRemoved = true;
        //        if (_playerPerdiodicTimeObserver != null)
        //        {
        //            LoadingActivityIndicator.Hidden = true;
        //            StubImageView.Hidden = true;

        //            AVPlayerViewControllerInstance.Player?.RemoveTimeObserver(_playerPerdiodicTimeObserver);
        //            _playerPerdiodicTimeObserver = null;
        //        }
        //    }
        //}

        private void StopVideo()
        {
            ViewModel?.PreviewVideoPlayer?.Stop();

            if (AVPlayerViewControllerInstance != null)
            {
                AVPlayerViewControllerInstance.Player = null;
            }
        }

        private void InitializeVideoControl()
        {
            AVPlayerViewControllerInstance = new AVPlayerViewController();
            AVPlayerViewControllerInstance.View.Frame = new CGRect(0, 0, VideoView.Frame.Width, VideoView.Frame.Height);
            AVPlayerViewControllerInstance.ShowsPlaybackControls = false;
            AVPlayerViewControllerInstance.VideoGravity = AVLayerVideoGravity.ResizeAspect;

            VideoView.Add(AVPlayerViewControllerInstance.View);
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
