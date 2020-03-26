using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("PlaceholderTextView"), DesignTimeVisible(true)]
    public class PlaceholderTextView : UITextView
    {
        private const float BorderWidth = 1;
        private const float FloatLabelX = 20;

        private readonly UIFont _floatPlaceholderFont = Theme.Font.RegularFontOfSize(12);

        private UILabel _floatingLabel;
        private CALayer _topBorderLine;
        private bool _isBorderInitilize;

        private NSAttributedString _attributedPlaceholder;
        public NSAttributedString AttributedPlaceholder
        {
            get => _attributedPlaceholder;
            set
            {
                _attributedPlaceholder = value;
                SetPlaceholderText(value.Value);
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                UpdatePlaceholderFrame();
            }
        }

        public PlaceholderTextView(CGRect frame)
            : base(frame)
        {
            InitializePlaceholder();
        }

        public PlaceholderTextView(IntPtr handle)
            : base(handle)
        {
            InitializePlaceholder();
        }

        protected override void Dispose(bool disposing)
        {
            if (_floatingLabel != null)
            {
                _floatingLabel.RemoveFromSuperview();
                _floatingLabel.Dispose();
                _floatingLabel = null;
            }

            Changed -= OnTextChanged;

            base.Dispose(disposing);
        }

        private void InitializePlaceholder()
        {
            _floatingLabel = new UILabel();
            AddSubview(_floatingLabel);
            Changed += OnTextChanged;
        }

        public void TryInitializeBorder()
        {
            if (_isBorderInitilize)
                return;

            _isBorderInitilize = true;

            Layer.BorderWidth = 0;
            TextContainer.MaximumNumberOfLines = 3;
            Layer.MasksToBounds = true;

            var borderY = _floatingLabel.Frame.Size.Height / 2;

            var leftLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, 1, Frame.Size.Height - borderY),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(leftLine);

            var rightLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(Frame.Size.Width - BorderWidth, borderY, BorderWidth, Frame.Size.Height - borderY),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(rightLine);

            var bottomLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, Frame.Size.Height - BorderWidth, Frame.Size.Width, 1),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(bottomLine);

            var topLeftLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, FloatLabelX, 1),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(topLeftLine);

            var floatPlaceholderSize = CalculateFloatPlaceholderSize(AttributedPlaceholder?.Value);
            var topRightLineX = floatPlaceholderSize.Width + topLeftLine.Bounds.Width;
            var topRightLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(topRightLineX, borderY, Frame.Size.Width - topRightLineX, 1),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(topRightLine);

            _topBorderLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, Frame.Size.Width, 1),
                BorderWidth = BorderWidth
            };
            Layer.AddSublayer(_topBorderLine);
        }

        private void SetPlaceholderText(string placeholder)
        {
            if (string.IsNullOrWhiteSpace(placeholder))
                return;

            _floatingLabel.Text = placeholder;

            _floatingLabel.AttributedText = AttributedPlaceholder;
            _floatingLabel.SizeToFit();
            _floatingLabel.Frame = new CGRect(FloatLabelX,
                                                _floatingLabel.Font.LineHeight,
                                                _floatingLabel.Frame.Size.Width,
                                                _floatingLabel.Frame.Size.Height);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdatePlaceholderFrame();
        }

        private void UpdatePlaceholderFrame()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                _floatingLabel.Font = _floatPlaceholderFont;
                _floatingLabel.TextColor = UIColor.FromCGColor(Layer.BorderColor);
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame = new CGRect(FloatLabelX,
                                                  0,
                                                  _floatingLabel.Frame.Size.Width,
                                                  _floatingLabel.Frame.Size.Height);

                _topBorderLine.Hidden = true;
            }
            else
            {
                _floatingLabel.AttributedText = AttributedPlaceholder;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame = new CGRect(14,
                                                  _floatingLabel.Font.LineHeight,
                                                  _floatingLabel.Frame.Size.Width,
                                                  _floatingLabel.Frame.Size.Height);
                _topBorderLine.Hidden = false;
            }
        }

        private CGSize CalculateFloatPlaceholderSize(string text)
        {
            return text.StringSize(_floatPlaceholderFont);
        }
    }
}
