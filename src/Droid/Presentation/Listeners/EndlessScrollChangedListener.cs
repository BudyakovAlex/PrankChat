using System;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class EndlessScrollChangedListener : RecyclerView.OnScrollListener, NestedScrollView.IOnScrollChangeListener
    {
        private OnLoadMoreListener _onLoadMoreListener;

        private int _scrolledDx;
        private int _scrolledDy;

        public EndlessScrollChangedListener()
        {
        }

        public EndlessScrollChangedListener(IntPtr ptr, JniHandleOwnership owner)
            : base(ptr, owner)
        {
        }

        public void SetOnLoadMoreListener(OnLoadMoreListener listener)
        {
            _onLoadMoreListener = listener;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            var oldDx = _scrolledDx;
            var oldDy = _scrolledDy;

            _scrolledDx += dx;
            _scrolledDy += dy;
            OnScrollChange(recyclerView, _scrolledDx, _scrolledDy, oldDx, oldDy);
        }

        public void OnScrollChange(NestedScrollView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            OnScrollChange((View)v, scrollX, scrollY, oldScrollX, oldScrollY);
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            var absScrollY = Math.Abs(scrollY);
            if ((absScrollY >= v.MeasuredHeight / 2)
                && (absScrollY > oldScrollY))
            {
                _onLoadMoreListener?.OnLoadMore();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onLoadMoreListener = null;
            }

            base.Dispose(disposing);
        }
    }
}