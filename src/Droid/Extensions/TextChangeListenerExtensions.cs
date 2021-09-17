using Android.Widget;
using Java.Lang;
using PrankChat.Mobile.Droid.Listeners;
using System;
using System.Linq;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class TextChangeListenerExtensions  
    {
        public static void SetTextChangeListened(this EditText editText, Action<ICharSequence> action)
        {
            editText.AddTextChangedListener(new TextChangeListener(editText, action));
        }
    }
}