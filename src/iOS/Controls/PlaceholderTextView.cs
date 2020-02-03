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
        public string Placeholder { get; set; }

        public UIColor PlaceholderColor { get; set; }

        private UIColor _textColor;
        public override UIColor TextColor
        {
            get => _textColor;
            set => _textColor = value;
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
            ShouldBeginEditing = t =>
            {
                if (Text == Placeholder)
                {
                    Text = string.Empty;
                    base.TextColor = _textColor;
                }

                return true;
            };

            ShouldEndEditing = t =>
            {
                if (string.IsNullOrEmpty(Text))
                {
                    Text = Placeholder;
                    base.TextColor = PlaceholderColor;
                }

                return true;
            };
        }

        private void UpdatePlaceholder()
        {
            if (Text == Placeholder)
            {
                Text = string.Empty;
                base.TextColor = _textColor;
            }
            else
            {
                Text = Placeholder;
                base.TextColor = PlaceholderColor;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            UpdatePlaceholder();
        }
    }
}
