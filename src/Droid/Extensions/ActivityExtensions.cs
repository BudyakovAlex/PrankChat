using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ActivityExtensions
    {
        public static void HideKeyboard(this Context context)
        {
            try
            {
                var activity = (Activity)context;
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
