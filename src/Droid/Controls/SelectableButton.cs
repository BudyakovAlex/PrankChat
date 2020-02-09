using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class SelectableButton : Button
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

        public float Transparency { get; set; } = 0.5f;

        #region Constructions

        public SelectableButton(Context context) : base(context)
        {
            Initialize();
        }

        public SelectableButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public SelectableButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public SelectableButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected SelectableButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            SetTextColor(ContextCompat.GetColorStateList(Application.Context, Resource.Color.accent));
        }

        private void ChangeSelectedState(bool isSelected)
        {
            Alpha = isSelected ? 1 : Transparency;
            if (isSelected)
            {
                SetBackgroundResource(Resource.Drawable.border_accent);
            }
            else
            {
                SetBackgroundResource(0);
            }
        }
    }
}
