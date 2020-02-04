using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("PlaceholderTextView"), DesignTimeVisible(true)]
    public class PlaceholderTextView : UITextView
    {
        private UILabel _placeholderLabel;
        private NSLayoutConstraint[] _placeholderConstraints;

        public string Placeholder
        {
            get => _placeholderLabel.Text;
            set => _placeholderLabel.Text = value;
        }

        public UIColor PlaceholderColor
        {
            get => _placeholderLabel.TextColor;
            set => _placeholderLabel.TextColor = value;
        }

        public UIFont PlaceholderFont
        {
            get => _placeholderLabel.Font;
            set => _placeholderLabel.Font = value;
        }

        public override UITextAlignment TextAlignment
        {
            get => base.TextAlignment;
            set
            {
                base.TextAlignment = value;
                _placeholderLabel.TextAlignment = value;
            }
        }

        public override UIEdgeInsets TextContainerInset
        {
            get => base.TextContainerInset;
            set
            {
                base.TextContainerInset = value;
                UpdateConstraintsForPlaceholder();
            }
        }

        #region Constructors

        public PlaceholderTextView()
        {
            Initialize();
        }

        public PlaceholderTextView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public PlaceholderTextView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected PlaceholderTextView(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal PlaceholderTextView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        #endregion

        void Initialize()
        {
            _placeholderLabel = new UILabel()
            {
                Lines = 0,
                BackgroundColor = UIColor.Clear,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = this.Font
            };

            ShouldBeginEditing = t =>
            {
                UpdatePlaceholder();
                return true;
            };

            ShouldEndEditing = t =>
            {
                UpdatePlaceholder();
                return true;
            };

            this.AddSubview(_placeholderLabel);
            UpdateConstraintsForPlaceholder();
        }

        private void UpdatePlaceholder()
        {
            _placeholderLabel.Hidden = string.IsNullOrWhiteSpace(this.Text);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _placeholderLabel.PreferredMaxLayoutWidth = TextContainer.Size.Width - TextContainer.LineFragmentPadding * 2.0f;
        }

        private void UpdateConstraintsForPlaceholder()
        {
            var newConstraints = new[]
            {
                NSLayoutConstraint.Create(_placeholderLabel, NSLayoutAttribute.Height, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Height, 1.0f, TextContainerInset.Top + TextContainerInset.Bottom),
                NSLayoutConstraint.Create(_placeholderLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 1.0f, -(TextContainerInset.Left + TextContainerInset.Right + TextContainer.LineFragmentPadding * 2.0f))
            };
            NSLayoutConstraint.ActivateConstraints(newConstraints);
            RemoveConstraints(_placeholderConstraints);
            _placeholderConstraints = newConstraints;
            AddConstraints(_placeholderConstraints);
        }
    }
}
