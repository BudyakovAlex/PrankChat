using System;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
	[Register("FloatPlaceholderTextField"), DesignTimeVisible(true)]
	public class FloatPlaceholderTextField : UITextField
	{
		private UILabel _floatingLabel;
		private bool _isBorderInitilize;

		[DisplayName("Label Color"), Export("FloatingLabelTextColor"), Browsable(true)]
		public UIColor FloatingLabelTextColor { get; set; } = UIColor.White;

		public UIFont FloatingLabelFont
		{
			get => _floatingLabel.Font;
			set => _floatingLabel.Font = value;
		}

		public override string Placeholder
		{
			get => base.Placeholder;
			set
			{
				base.Placeholder = value;
				SetPlaceholderText(value);
			}
		}

		public override NSAttributedString AttributedPlaceholder
		{
			get => base.AttributedPlaceholder;
			set
			{
				base.AttributedPlaceholder = value;
				SetPlaceholderText(value.Value);
			}
		}

		public FloatPlaceholderTextField(CGRect frame)
			: base(frame)
		{
			InitializePlaceholder();
		}

		public FloatPlaceholderTextField(IntPtr handle)
			: base(handle)
		{
			InitializePlaceholder();
		}

		public override CGRect ClearButtonRect(CGRect forBounds)
		{
			var rect = base.ClearButtonRect(forBounds);
			if (_floatingLabel == null)
				return rect;

			return new CGRect(rect.X,
							  rect.Y + _floatingLabel.Font.LineHeight / 2.0f,
							  rect.Size.Width,
							  rect.Size.Height);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			TryInitializeBorder();

			Action updateLabel = () =>
			{
				if (!string.IsNullOrEmpty(Text))
				{
					_floatingLabel.Alpha = 1.0f;
					_floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X,
													  -_floatingLabel.Frame.Size.Height / 2,
													  _floatingLabel.Frame.Size.Width,
													  _floatingLabel.Frame.Size.Height);

					Layer.BorderWidth = 0;
				}
				else
				{
					_floatingLabel.Alpha = 0.0f;
					_floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X,
													  _floatingLabel.Font.LineHeight,
													  _floatingLabel.Frame.Size.Width,
													  _floatingLabel.Frame.Size.Height);

					Layer.BorderWidth = 1;
				}
			};

			if (IsFirstResponder)
			{
                _floatingLabel.TextColor = UIColor.FromCGColor(Layer.BorderColor);

				var shouldFloat = !string.IsNullOrEmpty(Text);
				var isFloating = _floatingLabel.Alpha == 1f;

				if (shouldFloat == isFloating)
				{
					updateLabel();
				}
				else
				{
                    Animate(0.3f,
							0.0f,
							UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseOut,
							() => updateLabel(),
							() => { });
				}
			}
			else
			{
				_floatingLabel.TextColor = UIColor.FromCGColor(Layer.BorderColor);
				updateLabel();
			}
		}

        private void InitializePlaceholder()
		{
			_floatingLabel = new UILabel
			{
				Alpha = 0.0f,
				Font = UIFont.BoldSystemFontOfSize(12),
			};

			AddSubview(_floatingLabel);
			Placeholder = Placeholder;
		}

		private void TryInitializeBorder()
		{
			if (_isBorderInitilize)
				return;

			_isBorderInitilize = true;

			BorderStyle = UITextBorderStyle.None;
			var borderWidth = 1.0f;

			var leftLine = new CALayer
			{
				BorderColor = Layer.BorderColor,
				Frame = new CGRect(0, 0, 1, Frame.Size.Height),
				BorderWidth = borderWidth
			};
			Layer.AddSublayer(leftLine);

			var rightLine = new CALayer
			{
				BorderColor = Layer.BorderColor,
				Frame = new CGRect(Frame.Size.Width - 1, 0, 1, Frame.Size.Height),
				BorderWidth = borderWidth
			};
			Layer.AddSublayer(rightLine);

			var bottomLine = new CALayer
			{
				BorderColor = Layer.BorderColor,
				Frame = new CGRect(0, Frame.Size.Height - borderWidth, Frame.Size.Width, 1),
				BorderWidth = borderWidth
			};
			Layer.AddSublayer(bottomLine);

			var topLeftLine = new CALayer
			{
				BorderColor = Layer.BorderColor,
				Frame = new CGRect(0, 0, _floatingLabel.Frame.Location.X, 1),
				BorderWidth = borderWidth
			};
			Layer.AddSublayer(topLeftLine);

			var topRightLineX = _floatingLabel.Frame.Location.X + _floatingLabel.Frame.Width;
			var topRightLine = new CALayer
			{
				BorderColor = Layer.BorderColor,
				Frame = new CGRect(topRightLineX, 0, Frame.Width - topRightLineX, 1),
				BorderWidth = borderWidth
			};
			Layer.AddSublayer(topRightLine);
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
	}
}
