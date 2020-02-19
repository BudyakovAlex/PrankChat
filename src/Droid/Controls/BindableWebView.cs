using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Webkit;

namespace PrankChat.Mobile.Droid.Controls
{
    public class BindableWebView : WebView
    {
        private string _webViewContent;

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
            get { return _webViewContent; }
            set
            {
                _webViewContent = value;
                LoadHtmlString();
            }
        }

        private void Initilize()
        {
            Settings.JavaScriptEnabled = true;
        }

        private void LoadHtmlString()
        {
            LoadUrl(WebViewUrlContent);
        }
    }
}
