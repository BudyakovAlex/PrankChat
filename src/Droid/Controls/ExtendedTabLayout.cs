using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Google.Android.Material.Tabs;
using System;

namespace PrankChat.Mobile.Droid.Controls
{
    public class ExtendedTabLayout : TabLayout
    {
        public ExtendedTabLayout(Context context) : base(context)
        {
        }

        public ExtendedTabLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ExtendedTabLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected ExtendedTabLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public bool IsSelectionEnabled { get; set; }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return !IsSelectionEnabled;
        }
    }
}
