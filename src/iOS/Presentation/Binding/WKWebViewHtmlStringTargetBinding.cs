using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using WebKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class WKWebViewHtmlStringTargetBinding : MvxTargetBinding<WKWebView, string>
    {
        public WKWebViewHtmlStringTargetBinding(WKWebView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValue(string value)
        {
            Target.LoadHtmlString(value, null);
        }
    }
}
