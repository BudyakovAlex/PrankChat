using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.ViewPager.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class ApplicationSwipeViewPager : ViewPager
    {
        public ApplicationSwipeViewPager(Context context) : base(context)
        {
        }

        public ApplicationSwipeViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected ApplicationSwipeViewPager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }
    }
}
