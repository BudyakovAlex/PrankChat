using System;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class ViewOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly Func<View, MotionEvent, bool> touchFunc;

        public ViewOnTouchListener(Func<View, MotionEvent, bool> touchFunc)
        {
            this.touchFunc = touchFunc;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return touchFunc?.Invoke(v, e) ?? true;
        }
    }
}