using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Droid.Listeners;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class DelegeteViewOnClickExtension
    {
        public static void SetClickActionOnEditText(this EditText editText, Action<View> action)
        {
            editText.SetOnClickListener(new ViewOnClickListener(action));
        }
    }
}