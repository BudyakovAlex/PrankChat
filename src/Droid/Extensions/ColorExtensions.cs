using System.Drawing;
using AColor = Android.Graphics.Color;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ColorExtensions
    {
        public static AColor ToAndroidColor(this Color color) =>
            new AColor(color.R, color.G, color.B, color.A);
    }
}
