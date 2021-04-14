using System;
using AndroidX.RecyclerView.Widget;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
{
    public class RecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        private readonly Action<RecyclerView, int, int> _scrolledAction;

        public RecyclerViewScrollListener(Action<RecyclerView, int, int> scrolledAction)
        {
            _scrolledAction = scrolledAction;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);
            _scrolledAction?.Invoke(recyclerView, dx, dy);
        }
    }
}