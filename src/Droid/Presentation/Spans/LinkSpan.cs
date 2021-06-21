using System;
using Android.Text;
using Android.Text.Style;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Spans
{
    public class LinkSpan : ClickableSpan
    {
        private readonly Action<string> _action;

        private readonly string _text;
        private readonly bool _isUnderlineText;

        public LinkSpan(Action<string> action,
            string text = null,
            bool isUnderlineText = true)
        {
            _action = action;
            _text = text;
            _isUnderlineText = isUnderlineText;
        }

        public override void OnClick(View widget)
        {
            _action?.Invoke(_text);
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            base.UpdateDrawState(ds);
            ds.UnderlineText = _isUnderlineText;
        }
    }
}
