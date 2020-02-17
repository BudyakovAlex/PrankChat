using System;
using System.Reflection;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS.Presentation.Binding
{
    public class WebViewUrlTargetBinding : MvxPropertyInfoTargetBinding<WKWebView>
    {
        public static string TargetBinding = "Url";

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        public WebViewUrlTargetBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            if (target is WKWebView webView)
            {
                var nsurl = NSUrl.FromString(value.ToString());
                webView.LoadRequest(new NSUrlRequest(nsurl));
            }
            base.SetValueImpl(target, value);
        }
    }
}
