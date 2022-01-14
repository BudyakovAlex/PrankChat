using Android.Content;
using Android.Util;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class ContextExtensions
    {
        public static int GetHeightByAttribute(this Context context, int resourceId)
        {
            var typedValue = new TypedValue();
            if (context.Theme.ResolveAttribute(resourceId, typedValue, true))
            {
                var size = TypedValue.ComplexToDimensionPixelSize(typedValue.Data, context.Resources.DisplayMetrics);
                return size;
            }

            return 0;
        }
    }
}
