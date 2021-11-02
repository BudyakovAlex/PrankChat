﻿using System;
using Android.Runtime;
using Android.Webkit;

namespace PrankChat.Mobile.Droid.Controls.WebView
{
    public class CustomWebViewClient : WebViewClient
    {
        public Action<string> BeforeNavigationAction { get; set; }

        public CustomWebViewClient()
        {
        }

        protected CustomWebViewClient(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
        {
            BeforeNavigationAction?.Invoke(request.Url.ToString());
            view.LoadUrl(request.Url.ToString());
            return true;
        }
    }
}
