using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;

namespace PrankChat.Mobile.Droid.Presentation.Views.Publications
{
    public class PublicationsLinearLayoutManager : LinearLayoutManager
    {
        public event EventHandler LayoutCompleted;

        protected PublicationsLinearLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PublicationsLinearLayoutManager(Context context) : base(context)
        {
        }

        public PublicationsLinearLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public PublicationsLinearLayoutManager(Context context, int orientation, bool reverseLayout) : base(context, orientation, reverseLayout)
        {
        }

        public override void OnLayoutCompleted(RecyclerView.State state)
        {
            LayoutCompleted?.Invoke(this, null);

            base.OnLayoutCompleted(state);
        }
    }
}
