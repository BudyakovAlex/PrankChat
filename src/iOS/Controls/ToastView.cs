using System;
using Foundation;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    public partial class ToastView : UIView
    {
        private static readonly UINib Nib = UINib.FromName(nameof(ToastView), NSBundle.MainBundle);

        protected internal ToastView(IntPtr handle)
            : base(handle)
        {
        }

        public static ToastView Create(string text, ToastType toastType)
        {
            var toastView = (ToastView) Nib.Instantiate(null, null)[0];
            toastView.Label.Text = text;
            toastView.SetBackgroundColor(toastType);

            return toastView;
        }

        private void SetBackgroundColor(ToastType toastType)
        {
            BackgroundColor = GetBackgroundColor(toastType);
        }

        private UIColor GetBackgroundColor(ToastType toastType)
        {
            switch (toastType)
            {
                case ToastType.Positive:
                    return Theme.Color.PositiveToastBackground;

                case ToastType.Negative:
                    return Theme.Color.NegativeToastBackground;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}