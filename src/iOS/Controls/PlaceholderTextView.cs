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

        public override UIFont Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                _placeholderLabel.Font = value;
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

        public override string Text { get => base.Text; set { base.Text = value; UpdatePlaceholder(); } }

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

        private void Initialize()
        {
            _placeholderLabel = new UILabel()
            {
                Lines = 0,
                BackgroundColor = UIColor.Clear,
                TranslatesAutoresizingMaskIntoConstraints = false
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

            this.Changed += TextChanged;

            this.AddSubview(_placeholderLabel);
            UpdateConstraintsForPlaceholder();
        }

        private void UpdatePlaceholder()
        {
            _placeholderLabel.Hidden = !string.IsNullOrWhiteSpace(this.Text);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _placeholderLabel.PreferredMaxLayoutWidth = TextContainer.Size.Width - TextContainer.LineFragmentPadding * 2.0f;
        }

        private void UpdateConstraintsForPlaceholder()
        {
            if (_placeholderLabel == null)
                return;
            var newConstraints = new[]
            {
                
                NSLayoutConstraint.Create(_placeholderLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 17.0f),
                NSLayoutConstraint.Create(_placeholderLabel, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1.0f, 24.0f)
            };
            NSLayoutConstraint.ActivateConstraints(newConstraints);
            if (_placeholderConstraints != null)
                RemoveConstraints(_placeholderConstraints);
            _placeholderConstraints = newConstraints;
            AddConstraints(_placeholderConstraints);
        }

        private void TextChanged(object sender, EventArgs e)
        {
            UpdatePlaceholder();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_placeholderLabel != null)
            {
                _placeholderLabel.RemoveFromSuperview();
                _placeholderLabel.Dispose();
                _placeholderLabel = null;
            }

            this.Changed -= TextChanged;
        }
    }
}
