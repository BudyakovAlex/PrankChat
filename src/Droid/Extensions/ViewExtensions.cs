using Android.Graphics;
using Android.Graphics.Drawables;
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

        public static GradientDrawable AddBorder(
            this View view,
            float topLeftRadius,
            float topRightRadius,
            float bottomLeftRadius,
            float bottomRightRadius,
            float widthBorder,
            Color borderColor,
            Color backgroundColor)
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetColor(backgroundColor.ToArgb());
            gradientDrawable.SetStroke((int)widthBorder, borderColor);
            gradientDrawable.SetCornerRadii(new[] {
                    topLeftRadius,
                    topLeftRadius,
                    topRightRadius,
                    topRightRadius,
                    bottomLeftRadius,
                    bottomLeftRadius,
                    bottomRightRadius,
                    bottomRightRadius });
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean)
            {
                view.SetBackgroundDrawable(gradientDrawable);
            }
            else
            {
                view.Background = gradientDrawable;
            }

            return gradientDrawable;
        }
    }
}