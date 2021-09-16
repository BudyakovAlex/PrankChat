using Android.Widget;
using Java.Lang;
using PrankChat.Mobile.Droid.Listeners;
using System;
using System.Linq;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class DelegateTextChangeListenerExtensions  
    {
        public static void SetActionOnEditText(this EditText editText, Action<EditText, ICharSequence> action)
        {
            editText.AddTextChangedListener(new TextChangeListener<EditText>(action, editText));
        }
    }
}