using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ActivityExtensions
    {
        public static void HideKeyboard(this Activity activity)
        {
            var imm = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            imm?.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}
