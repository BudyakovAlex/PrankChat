using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register(nameof(SelfSizeTableView))]
    public class SelfSizeTableView : UITableView
    {
        public SelfSizeTableView()
        {
        }

        public SelfSizeTableView(NSCoder coder) : base(coder)
        {
        }

        public SelfSizeTableView(CGRect frame) : base(frame)
        {
        }

        public SelfSizeTableView(CGRect frame, UITableViewStyle style) : base(frame, style)
        {
        }

        protected SelfSizeTableView(NSObjectFlag t) : base(t)
        {
        }

        protected internal SelfSizeTableView(IntPtr handle) : base(handle)
        {
        }

        public override CGSize IntrinsicContentSize => base.ContentSize;

        public override CGSize ContentSize
        {
            get => base.ContentSize;
            set
            {
                base.ContentSize = value;
                InvalidateIntrinsicContentSize();
            }
        }
    }
}