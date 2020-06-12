using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Java.Util.Logging;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ActivityExtensions
    {
        public static void HideKeyboard(this Activity activity)
        {
            try
            {
                var inputMethodManager = InputMethodManager.FromContext(activity);
                if (inputMethodManager != null)
                {
                    var token = activity.CurrentFocus?.WindowToken;
                    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                    activity.Window.DecorView.ClearFocus();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
