using CoreAnimation;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class CALayerExtensions
    {
        public static void RemoveSublayers(this CALayer layer)
        {
            foreach (var sublayer in layer.Sublayers)
            {
                sublayer.RemoveFromSuperLayer();
            }
        }
    }
}
