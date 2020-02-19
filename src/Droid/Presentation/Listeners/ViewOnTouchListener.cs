using System;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    //NOTE: use instead click when cell click invokes only after second click, this listener avoid current problem
    public class ViewOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly Func<View, MotionEvent, bool> _touchFunc;

        public ViewOnTouchListener(Func<View, MotionEvent, bool> touchFunc)
        {
            _touchFunc = touchFunc;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return _touchFunc?.Invoke(v, e) ?? true;
        }
    }
}