using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using static Google.Android.Material.Tabs.TabLayout;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class BindingExtensions
    {
        public static string BindBackgroundDrawable(this View _)
            => nameof(BackgroundDrawableBinding);

        public static string BindBackgroundResource(this View _)
            => nameof(BackgroundResourceBinding);

        public static string BindBackgroundColor(this View _)
            => nameof(BackgroundColorBinding);

        public static string BindOrderButtonStyle(this AppCompatButton _)
            => nameof(OrderButtonStyleBinding);

        public static string BindVideoUrl(this VideoView _)
            => nameof(VideoUrlTargetBinding);

        public static string BindTouch(this FrameLayout _)
            => nameof(ViewTouchTargetBinding);

        public static string BindTintColor(this ImageView _)
            => nameof(ImageViewTintColorTargetBinding);

        public static string BindTextColor(this TextView _)
            => nameof(TextColorTargetBinding);

        public static string BindTabText(this Tab _)
            => nameof(TabLayoutTabTextBinding);

        public static string BindPaddingStart(this View _)
            => PaddingTargetBinding.StartPadding;

        public static string BindPaddingTop(this View _)
            => PaddingTargetBinding.TopPadding;

        public static string BindPaddingBottom(this View _)
            => PaddingTargetBinding.BottomPadding;

        public static string BindPaddingEnd(this View _)
            => PaddingTargetBinding.EndPadding;
    }
}