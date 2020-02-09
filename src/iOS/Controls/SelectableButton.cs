using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("SelectableButton"), DesignTimeVisible(true)]
    public class SelectableButton : UIButton
    {
        public override bool Selected
        {
            get => base.Selected;
            set
            {
                base.Selected = value;
                ChangeSelectedState(value);
            }
        }

        public UIColor Color { get; set; } = AppTheme.Theme.Color.Accent;

        public float BorderWidth { get; set; } = 1;

        public float Transparency { get; set; } = 0.5f;

        #region Constructions

        public SelectableButton()
        {
            Initialize();
        }

        public SelectableButton(UIButtonType type) : base(type)
        {
            Initialize();
        }

        public SelectableButton(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public SelectableButton(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected SelectableButton(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal SelectableButton(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            Layer.BorderColor = Color.CGColor;
            Layer.CornerRadius = 4;
            SetTitleColor(Color, UIControlState.Normal);
        }

        private void ChangeSelectedState(bool isSelected)
        {
            Layer.BorderWidth = isSelected ? BorderWidth : 0;
            Alpha = isSelected ? 1 : Transparency;
        }
    }
}
