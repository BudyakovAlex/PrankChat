using System;
using CoreGraphics;
using Foundation;
using UIKit;
using static PrankChat.Mobile.iOS.AppTheme.Theme;

namespace PrankChat.Mobile.iOS.Controls
{
    public class EmptyView : UIView
    {
        private UILabel _titleLabel;
        private UIImageView _imageView;

        protected EmptyView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        protected EmptyView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected EmptyView(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal EmptyView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public string Title
        {
            get => _titleLabel.Text;
            set => _titleLabel.Text = value;
        }

        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                _imageView.Image = UIImage.FromBundle(_imageName);
            }
        }

        public static EmptyView Create(string title, string imageName) =>
            new EmptyView(CGRect.Empty)
            {
                Title = title,
                ImageName = imageName
            };

        public EmptyView AttachToViewByConstraints(UIView parentView)
        {
            parentView.AddSubview(parentView);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                TopAnchor.ConstraintEqualTo(parentView.TopAnchor),
                BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor),
                LeadingAnchor.ConstraintEqualTo(parentView.LeadingAnchor),
                TrailingAnchor.ConstraintEqualTo(parentView.TrailingAnchor)
            });

            return this;
        }

        public EmptyView AttachToTableViewAsBackgroundView(UITableView tableView)
        {
            TranslatesAutoresizingMaskIntoConstraints = true;

            tableView.BackgroundView = this;
            Bounds = tableView.Bounds;
            SetNeedsLayout();
            LayoutIfNeeded();

            return this;
        }

        private void Initialize()
        {
            BackgroundColor = Color.White;
            TranslatesAutoresizingMaskIntoConstraints = false;

            _imageView = new UIImageView { TranslatesAutoresizingMaskIntoConstraints = false };
            _titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = Font.RegularFontOfSize(12),
                TextColor = Color.Gray,
                TextAlignment = UITextAlignment.Center
            };

            var containerView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false};
            AddSubview(containerView);
            containerView.AddSubviews(_imageView, _titleLabel);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                containerView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 20f),
                containerView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -20f),
                containerView.CenterXAnchor.ConstraintEqualTo(CenterXAnchor),
                containerView.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
                _titleLabel.TopAnchor.ConstraintEqualTo(containerView.TopAnchor),
                _titleLabel.LeadingAnchor.ConstraintEqualTo(containerView.LeadingAnchor),
                _titleLabel.TrailingAnchor.ConstraintEqualTo(containerView.TrailingAnchor),
                _imageView.TopAnchor.ConstraintEqualTo(_titleLabel.BottomAnchor, 24f),
                _imageView.CenterXAnchor.ConstraintEqualTo(containerView.CenterXAnchor),
                _imageView.BottomAnchor.ConstraintEqualTo(containerView.BottomAnchor),
            });
        }
    }
}