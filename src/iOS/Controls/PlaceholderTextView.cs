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
        private UILabel _floatingLabel;
        private bool _isBorderInitilize;

        private CALayer _topBorderLine;

        [DisplayName("Label Color"), Export("FloatingLabelTextColor"), Browsable(true)]
        public UIColor FloatingLabelTextColor { get; set; } = UIColor.White;

        public UIFont FloatingLabelFont
        {
            get => _floatingLabel.Font;
            set => _floatingLabel.Font = value;
        }

        private string _placeholder;
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                SetPlaceholderText(value);
            }
        }

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
                _floatingLabel.TextColor = UIColor.FromCGColor(Layer.BorderColor);
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
            _floatingLabel = new UILabel
            {
                Font = Theme.Font.RegularFontOfSize(12),
            };

            AddSubview(_floatingLabel);
            Placeholder = Placeholder;

            Layer.BorderWidth = 0;

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

            var borderY = _floatingLabel.Frame.Height / 2;

            var borderWidth = 1.0f;
            var leftLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, 1, Frame.Height - borderY),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(leftLine);

            var rightLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(Frame.Width - borderWidth, borderY, borderWidth, Frame.Height - borderY),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(rightLine);

            var bottomLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, Frame.Height - borderWidth, Frame.Width, 1),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(bottomLine);

            var topLeftLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, _floatingLabel.Frame.Location.X, 1),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(topLeftLine);

            var topRightLineX = _floatingLabel.Frame.Location.X + _floatingLabel.Frame.Width;
            var topRightLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(topRightLineX, borderY, Frame.Width - topRightLineX, 1),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(topRightLine);

            _topBorderLine = new CALayer
            {
                BorderColor = Layer.BorderColor,
                Frame = new CGRect(0, borderY, Frame.Width, 1),
                BorderWidth = borderWidth
            };
            Layer.AddSublayer(_topBorderLine);
        }

        private void SetPlaceholderText(string placeholder)
        {
            if (string.IsNullOrWhiteSpace(placeholder))
                return;

            _floatingLabel.Text = placeholder;
            _floatingLabel.SizeToFit();
            _floatingLabel.Frame = new CGRect(20,
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
            //TryInitializeBorder();
            if (!string.IsNullOrEmpty(Text))
            {
                _floatingLabel.Font = Theme.Font.RegularFontOfSize(12);
                _floatingLabel.Frame = new CGRect(20,
                                                  0,
                                                  _floatingLabel.Frame.Size.Width,
                                                  _floatingLabel.Frame.Size.Height);

                _topBorderLine.Hidden = true;
            }
            else
            {
                _floatingLabel.Font = Theme.Font.RegularFontOfSize(Font.PointSize);
                _floatingLabel.Frame = new CGRect(14,
                                                  _floatingLabel.Font.LineHeight,
                                                  _floatingLabel.Frame.Size.Width,
                                                  _floatingLabel.Frame.Size.Height);

                _topBorderLine.Hidden = false;
            }
        }
    }
}
