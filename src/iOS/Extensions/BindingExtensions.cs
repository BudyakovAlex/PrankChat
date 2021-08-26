using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Presentation.Binding;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class BindingExtensions
    {
        public static string BindSelected(this UIButton _)
            => nameof(UIButtonSelectedTargetBinding);

        public static string BindOrderType(this UIButton _)
            => nameof(UIButtonOrderTypeTargetBinding);

        public static string BindHtmlString(this WKWebView _)
            => nameof(WKWebViewHtmlStringTargetBinding);

        public static string BindOrderType(this UIImageView _)
            => nameof(UIImageViewOrderTypeTargetBinding);

        public static string BindStartPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding;

        public static string BindEndPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding;

        public static string BindTopPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding;

        public static string BindBottomPadding(this FloatPlaceholderTextField _)
            => FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding;
    }
}
