using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using AndroidX.Core.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class ClearEditText : EditText
    {
        private Drawable clearButton;

        protected ClearEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ClearEditText(Context context) : base(context)
        {
            Init();
        }

        public ClearEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(attrs);
        }

        public ClearEditText(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(attrs);
        }

        private void Init(IAttributeSet attrs = null)
        {
            clearButton = ContextCompat.GetDrawable(Android.App.Application.Context, Resource.Drawable.ic_clear);
            clearButton.SetBounds(0, 0, clearButton.IntrinsicWidth, clearButton.IntrinsicHeight);

            SetupEvents();
        }

        private void SetupEvents()
        {
            // Handle clear button visibility
            TextChanged += (sender, e) =>
            {
                UpdateClearButton();
            };

            FocusChange += (sender, e) =>
            {
                UpdateClearButton(e.HasFocus);
            };

            // Handle clearing the text
            Touch += (sender, e) =>
            {
                if (GetCompoundDrawables()[2] == null || e.Event.Action != MotionEventActions.Up)
                {
                    e.Handled = false;
                    return;
                }
                if (e.Event.GetX() > (Width - PaddingRight - clearButton.IntrinsicWidth))
                {
                    Text = "";
                    UpdateClearButton();
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            };
        }

        private void UpdateClearButton(bool hasFocus = true)
        {
            var compoundDrawables = GetCompoundDrawables();
            var compoundDrawable = Text.Length == 0 || !hasFocus ? null : clearButton;
            SetCompoundDrawables(compoundDrawables[0], compoundDrawables[1], compoundDrawable, compoundDrawables[3]);
        }
    }
}
