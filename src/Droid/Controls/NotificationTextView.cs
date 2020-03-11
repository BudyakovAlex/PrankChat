using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Widget;

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

        private string _descriptionText;
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                _descriptionText = value;
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
            string s = SetString("", _profileName);
            s = SetString(s, _title, "  ");
            int length = s.Length;
            s = SetString(s, _descriptionText, $"{Environment.NewLine}{Environment.NewLine}");

            var ss = new SpannableString(s);
            ss.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), 0, _profileName?.Length ?? 0, SpanTypes.ExclusiveExclusive);
            if (s.Length > length)
                ss.SetSpan(new RelativeSizeSpan(0.6f), length, length + 2, SpanTypes.ExclusiveExclusive);
            this.SetText(ss, TextView.BufferType.Spannable);
        }

        private string SetString(string first, string second, string delimiter = "")
        {
            if (!string.IsNullOrWhiteSpace(second))
            {
                if (!string.IsNullOrWhiteSpace(first))
                    first += delimiter;
                first += second;
            }
            return first;
        }
    }
}
