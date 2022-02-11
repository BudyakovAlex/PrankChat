using PrankChat.Mobile.iOS.Binding;
using PrankChat.Mobile.iOS.Controls;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class BindingExtensions
    {
        public static string BindSelected(this UIButton _)
            => nameof(UIButtonSelectedTargetBinding);

        public static string BindOrderButtonStyle(this UIButton _)
            => nameof(UIButtonOrderTypeTargetBinding);

        public static string BindHtmlString(this WKWebView _)
            => nameof(WKWebViewHtmlStringTargetBinding);

        public static string BindOrderImageStyle(this UIImageView _)
            => nameof(UIImageViewOrderTypeTargetBinding);

        public static string BindAttributedText(this UILabel _)
            => nameof(UILabelAttributedTextTargetBinding);

        public static string BindStartPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding;

        public static string BindEndPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding;

        public static string BindTopPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding;

        public static string BindBottomPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding;

        public static string BindIsEnabled(this UITabBarController _)
           => nameof(UITabBarControllerIsEnabedTargetBinding);
    }
}
