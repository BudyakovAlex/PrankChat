using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Droid.Listeners;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class DelegeteViewOnClickExtension
    {
        public static void SetClickActionOnEditText(this View view, Action<View> action)
        {
            view.SetOnClickListener(new ViewOnClickListener(action));
        }
    }
}