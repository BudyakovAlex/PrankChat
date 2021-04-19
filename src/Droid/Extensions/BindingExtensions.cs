using Android.Views;
using PrankChat.Mobile.Droid.Presentation.Bindings;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class BindingExtensions
    {
        public static string BindDrawable(this View _) => BackgroundBinding.TargetBinding; 
    }
}