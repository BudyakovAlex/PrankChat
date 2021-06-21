using System;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Droid.Presentation.Listeners
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