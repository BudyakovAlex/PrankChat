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
        private UIColor _textColor;
        public string Placeholder { get; set; }

        public UIColor PlaceholderColor { get; set; }

        public override UIColor TextColor
        {
            get => _textColor;
            set => _textColor = value;
        }

        #region Constructors

        public PlaceholderTextView()
        {
        }

        public PlaceholderTextView(NSCoder coder) : base(coder)
        {
        }

        public PlaceholderTextView(CGRect frame) : base(frame)
        {
        }

        protected PlaceholderTextView(NSObjectFlag t) : base(t)
        {
        }

        protected internal PlaceholderTextView(IntPtr handle) : base(handle)
        {
            Started += CustomTextViewStarted;
            Ended += CustomTextViewEnded;
        }

        #endregion

        private void CustomTextViewStarted(object sender, EventArgs e)
        {
            TryRemovePlaceholder();
        }

        private void CustomTextViewEnded(object sender, EventArgs e)
        {
            UpdatePlaceholder();
        }

        public override bool BecomeFirstResponder()
        {
            UpdatePlaceholder();

            return base.BecomeFirstResponder();
        }

        private void TryRemovePlaceholder()
        {
            if (Text == Placeholder)
            {
                Text = string.Empty;
            }
        }

        private void UpdatePlaceholder()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = Placeholder;
                base.TextColor = PlaceholderColor;
                return;
            }

            base.TextColor = _textColor;
        }

        public override void LayoutSubviews()
        {
            UpdatePlaceholder();

            base.LayoutSubviews();
        }
    }
}
