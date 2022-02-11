using System.Drawing;
using UIKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class ColorExtensions
    {
        public static UIColor ToUIColor(this Color color) =>
            UIColor.FromRGBA(color.R, color.G, color.B, color.A);
    }
}
