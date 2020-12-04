using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register(nameof(CheckBox))]
    public class CheckBox : UIButton
    {
        private const string IconChecked = "ic_checkbox_checked";
        private const string IconUnchecked = "ic_checkbox_unchecked";
        private const float SizeCheckBox = 18f;

        public event EventHandler IsCheckedChanged;

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                IsCheckedChanged.Raise(this);
                UpdateCheckImage();
            }
        }

        public CheckBox()
        {
            Initialize();
        }

        public CheckBox(UIButtonType type) : base(type)
        {
            Initialize();
        }

        public CheckBox(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public CheckBox(CGRect frame) : base(frame)
        {
            Initialize();
        }

        //public CheckBox(CGRect frame, UIAction primaryAction) : base(frame, primaryAction)
        //{
        //    Initialize();
        //}

        protected CheckBox(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal CheckBox(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Initialize();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            IsChecked = !IsChecked;
        }

        public void SwitchChecked()
        {
            IsChecked = !IsChecked;
        }

        private void Initialize()
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                HeightAnchor.ConstraintEqualTo(SizeCheckBox),
                WidthAnchor.ConstraintEqualTo(SizeCheckBox)
            });
        }

        private void UpdateCheckImage()
        {
            var image = IsChecked ? IconChecked : IconUnchecked;
            SetImage(UIImage.FromBundle(image), UIControlState.Normal);
        }
    }
}
