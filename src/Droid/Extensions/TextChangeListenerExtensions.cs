using Android.Widget;
using Java.Lang;
using PrankChat.Mobile.Droid.Listeners;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class TextChangeListenerExtensions  
    {
        public static void SetTextChangeListener(this EditText editText, Action<ICharSequence> action)
        {
            editText.AddTextChangedListener(new TextChangeListener(action));
        }
    }
}