using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Droid.Support.V7.RecyclerView;
using PrankChat.Mobile.Droid.Presentation.Listeners;

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

        public ICommand LoadMoreCommand { get; set; }

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
            }
            else
            {
                RemoveOnScrollListener(_scrollChangedListener);
            }
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
            LoadMoreCommand?.Execute(null);
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