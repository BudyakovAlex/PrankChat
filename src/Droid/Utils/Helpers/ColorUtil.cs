using System;
using Android.Graphics;

namespace PrankChat.Mobile.Droid.Utils.Helpers
{
    public static class ColorUtil
    {
        public static Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}
