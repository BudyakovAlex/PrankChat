using PrankChat.Mobile.iOS.Binding;
using UIKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class BindingExtensions
    {
        public static string BindSelected(this UIButton _)
            => UIButtonSelectedTargetBinding.TargetBinding;
    }
}
