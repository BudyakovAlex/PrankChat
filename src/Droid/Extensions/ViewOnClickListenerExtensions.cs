using Android.Views;
using PrankChat.Mobile.Droid.Listeners;
using System;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ViewOnClickListenerExtensions
    {
        public static void SetOnClickListener(this View view, Action<View> action) =>
            view.SetOnClickListener(new ViewOnClickListener(action));
    }
}