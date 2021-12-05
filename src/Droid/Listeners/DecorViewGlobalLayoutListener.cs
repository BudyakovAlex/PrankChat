using System;
using Android.Graphics;
using Android.Views;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class DecorViewGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly View _decorView;

        private int _maxHeight;
        private bool _keyboardShowed;

        public DecorViewGlobalLayoutListener(View decorView)
        {
            _decorView = decorView;
        }

        public void OnGlobalLayout()
        {
            var rect = new Rect();
            _decorView.GetWindowVisibleDisplayFrame(rect);
            var content = _decorView.FindViewById(Window.IdAndroidContent);
            if (rect.Top != 0)
            {
                _maxHeight = Math.Max(rect.Bottom, _maxHeight);
            }

            var heightDiff = _maxHeight - rect.Top - content.Bottom;
            if (heightDiff <= 0)
            {
                if (!_keyboardShowed)
                {
                    return;
                }

                _keyboardShowed = false;
                _decorView.ClearFocus();
            }
            else
            {
                _keyboardShowed = true;
            }
        }
    }
}
