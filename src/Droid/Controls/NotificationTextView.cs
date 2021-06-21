using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Widget;
using Java.Interop;
using Java.Lang;

namespace PrankChat.Mobile.Droid.Controls
{
    public class NotificationTextView : TextView
    {
        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                SetDataToLabel();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SetDataToLabel();
            }
        }


        public NotificationTextView(Context context) : base(context)
        {
        }

        public NotificationTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NotificationTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public NotificationTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected NotificationTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void SetDataToLabel()
        {
            var haveProfileName = !string.IsNullOrWhiteSpace(_profileName);
            var text = string.Join(haveProfileName ? "  " : "", _profileName, _title);
            var spannableString = new SpannableString(text);
            spannableString.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), 0, _profileName?.Length ?? 0, SpanTypes.ExclusiveExclusive);
            this.SetText(spannableString, TextView.BufferType.Spannable);
        }
    }
}
