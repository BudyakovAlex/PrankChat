namespace PrankChat.Mobile.Droid.Utils.Helpers
{
    public static class DisplayUtils
    {
        public static int DpToPx(int dp)
        {
            var metrics = Android.Content.Res.Resources.System.DisplayMetrics;
            return (int)(dp * metrics.Density);
        }

        public static float PxToDp(int px)
        {
            return px / Android.Content.Res.Resources.System.DisplayMetrics.Density;
        }
    }
}