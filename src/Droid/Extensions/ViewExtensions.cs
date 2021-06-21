using Android.Views;
using PrankChat.Mobile.Droid.Providers;

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

        public static void SetRoundedCorners(this View layout, float radiusPx)
        {
            layout.OutlineProvider = new OutlineProvider((view, outline) =>
            {
                outline.SetRoundRect(0, 0, view.MeasuredWidth, view.MeasuredHeight, radiusPx);
            });

            layout.ClipToOutline = true;
        }
    }
}