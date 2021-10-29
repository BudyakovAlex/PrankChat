using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Webkit;

namespace PrankChat.Mobile.Droid.Controls.WebView
{
    public class BindableWebView : Android.Webkit.WebView
    {
        private string _webViewContent;
        private CustomWebViewClient _webViewClient;

        public BindableWebView(Context context) : base(context)
        {
            Initilize();
        }

        public BindableWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initilize();
        }

        public BindableWebView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initilize();
        }

        public BindableWebView(Context context, IAttributeSet attrs, int defStyleAttr, bool privateBrowsing) : base(context, attrs, defStyleAttr, privateBrowsing)
        {
            Initilize();
        }

        public BindableWebView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initilize();
        }

        protected BindableWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initilize();
        }

        public string WebViewUrlContent
        {
            get => _webViewContent;
            set
            {
                _webViewContent = value;
                LoadHtmlString();
            }
        }

        public void SetOnBeforeNavigation(Action<string> action)
        {
            _webViewClient.OnBeforeNavigation = action;
        }

        private void Initilize()
        {
            Settings.JavaScriptEnabled = true;
            Settings.AllowUniversalAccessFromFileURLs = true;
            _webViewClient = new CustomWebViewClient();
            SetWebViewClient(_webViewClient);
        }

        private void LoadHtmlString()
        {
            LoadUrl(WebViewUrlContent);
        }
    }
}
