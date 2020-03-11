using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace PrankChat.Mobile.Droid.Controls
{
    public class NotificationTextView : TextView
    {
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
    }
}
