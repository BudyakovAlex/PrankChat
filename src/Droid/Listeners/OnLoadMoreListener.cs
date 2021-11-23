using System;
using PrankChat.Mobile.Core.Extensions;

namespace PrankChat.Mobile.Droid.Listeners
{
    public class OnLoadMoreListener
    {
        private readonly Action _onLoadMore;

        public OnLoadMoreListener(Action onLoadMore)
        {
            onLoadMore.ThrowIfNull();

            _onLoadMore = onLoadMore;
        }

        public void OnLoadMore()
        {
            _onLoadMore();
        }
    }
}