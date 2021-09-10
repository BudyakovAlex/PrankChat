using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.SourcesAndDelegates
{
    public abstract class PagedTableViewSource : MvxTableViewSource
    {
        private const double NextPageScrollThreshold = 0.7;

        protected PagedTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public MvxAsyncCommand LoadMoreItemsCommand { get; set; }

        public bool HasNextPage { get; set; } = true;

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            if (!CanMoreItems(scrollView))
            {
                return;
            }

            LoadMoreItemsCommand?.Execute(null);
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (!CanMoreItems(scrollView))
            {
                return;
            }

            LoadMoreItemsCommand?.Execute(null);
        }

        protected virtual bool CanMoreItems(UIScrollView scrollView)
        {
            if (!HasNextPage ||
                LoadMoreItemsCommand is null ||
                LoadMoreItemsCommand.IsRunning)
            {
                return false;
            }

            return (scrollView.ContentOffset.Y / scrollView.ContentSize.Height) >= NextPageScrollThreshold;
        }
    }
}