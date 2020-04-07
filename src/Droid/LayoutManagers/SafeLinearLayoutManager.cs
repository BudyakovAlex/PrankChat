using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Java.Lang;

namespace PrankChat.Mobile.Droid.LayoutManagers
{
    public class SafeLinearLayoutManager : LinearLayoutManager
    {
        public SafeLinearLayoutManager(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public SafeLinearLayoutManager(Context context)
            : base(context)
        {
        }

        public SafeLinearLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public SafeLinearLayoutManager(Context context, int orientation, bool reverseLayout)
            : base(context, orientation, reverseLayout)
        {
        }

        public override void OnLayoutChildren(RecyclerView.Recycler recycler, RecyclerView.State state)
        {
            try
            {
                base.OnLayoutChildren(recycler, state);
            }
            catch (IndexOutOfBoundsException e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
        }
    }
}