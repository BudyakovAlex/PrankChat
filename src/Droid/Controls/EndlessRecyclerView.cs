using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Commands;
using MvvmCross.DroidX.RecyclerView;
using PrankChat.Mobile.Droid.Listeners;
using System;

namespace PrankChat.Mobile.Droid.Controls
{
    public class EndlessRecyclerView : MvxRecyclerView
    {
        public const int DefaultStyle = -1;

        private EndlessScrollChangedListener _scrollChangedListener;
        private LinearLayoutManager _linearLayoutManager;

        protected EndlessRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public EndlessRecyclerView(Context context) : this(context, null)
        {
        }

        public EndlessRecyclerView(Context context, IAttributeSet attrs)
            : this(context, attrs, DefaultStyle)
        {
        }

        public EndlessRecyclerView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        public MvxAsyncCommand LoadMoreItemsCommand { get; set; }

        private bool _hasNextPage;
        public bool HasNextPage
        {
            get => _hasNextPage;
            set
            {
                if (_hasNextPage == value)
                {
                    return;
                }

                _hasNextPage = value;
                SetOnScrollListenerState();
            }
        }

        private void SetOnScrollListenerState()
        {
            if (_hasNextPage)
            {
                AddOnScrollListener(_scrollChangedListener);
                return;
            }

            RemoveOnScrollListener(_scrollChangedListener);
        }

        private void Initialize(Context context)
        {
            _scrollChangedListener = new EndlessScrollChangedListener();
            _scrollChangedListener.SetOnLoadMoreListener(new OnLoadMoreListener(OnLoadMore));

            _linearLayoutManager = new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
            SetLayoutManager(_linearLayoutManager);
        }

        private void OnLoadMore()
        {
            LoadMoreItemsCommand?.Execute();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetOnScrollChangeListener(null);
                RemoveOnScrollListener(_scrollChangedListener);

                _scrollChangedListener?.SetOnLoadMoreListener(null);
                _scrollChangedListener = null;
            }

            base.Dispose(disposing);
        }
    }
}