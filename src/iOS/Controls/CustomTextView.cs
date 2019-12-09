using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("CustomTextView"), DesignTimeVisible(true)]
    public class CustomTextView : UITextView
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

        public CustomTextView()
        {
        }

        public CustomTextView(NSCoder coder) : base(coder)
        {
        }

        public CustomTextView(CGRect frame) : base(frame)
        {
        }

        protected CustomTextView(NSObjectFlag t) : base(t)
        {
        }

        protected internal CustomTextView(IntPtr handle) : base(handle)
        {
            Started += CustomTextView_Started;
            Ended += CustomTextView_Ended;
        }

        #endregion

        private void CustomTextView_Started(object sender, EventArgs e)
        {
            TryRemovePlaceholder();
        }

        private void CustomTextView_Ended(object sender, EventArgs e)
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
