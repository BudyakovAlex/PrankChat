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
        public static string BindDrawable(this View _) => nameof(BackgroundBinding); 
        public static string BindResource(this FrameLayout _) => nameof(BackgroundResourceBinding);
        public static string BindColor(this View _) => nameof(BackgroundColorBinding);
        public static string BindOrderButtonStyle(this AppCompatButton _) => nameof(OrderButtonStyleBinding);
        public static string BindVideoUrl(this ExtendedVideoView _) => nameof(VideoUrlTargetBinding);
        public static string BindViewTouch(this FrameLayout _) => nameof(ViewTouchTargetBinding);
        public static string BindImageViewTintColor(this ImageView _) => nameof(ImageViewTintColorTargetBinding);
        public static string BindTextColor(this TextView _) => nameof(TextColorTargetBinding);
        public static string BindTabLayoutTabText(this Tab _) => nameof(TabLayoutTabTextBinding);
        public static string BindPaddingStart(this View _) => PaddingTargetBinding.StartPadding;
        public static string BindPaddingTop(this View _) => PaddingTargetBinding.TopPadding;
        public static string BindPaddingBottom(this View _) => PaddingTargetBinding.BottomPadding;
        public static string BindPaddingEnd(this View _) => PaddingTargetBinding.EndPadding;
    }
}