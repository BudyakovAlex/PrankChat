using Android.Views;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static (int x, int y) GetLocationInWindow(this View view)
        {
            var location = new int[2];
            view.GetLocationInWindow(location);

            return (location[0], location[1]);
        }

        public static (int x, int y) GetLocationOnScreen(this View view)
        {
            var location = new int[2];
            view.GetLocationOnScreen(location);

            return (location[0], location[1]);
        }
    }
}